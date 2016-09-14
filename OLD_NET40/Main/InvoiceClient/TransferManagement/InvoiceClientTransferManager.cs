using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using InvoiceClient.Properties;
using Model.Schema.EIVO;
using System.Xml;
using InvoiceClient.Agent;
using Utility;

namespace InvoiceClient.TransferManagement
{
    public static class InvoiceClientTransferManager
    {
        private static ServiceController _ServiceController;
        private static Dictionary<Type, ITransferManager> _ManagerInstance;
        private static Dictionary<Type, ServerInspector> _InspectorInstance;
        private static List<Type> _MainTabs;

        static InvoiceClientTransferManager()
        {
            _ManagerInstance = new Dictionary<Type, ITransferManager>();
            if (!String.IsNullOrEmpty(Settings.Default.TransferManager))
            {
                foreach (String typeName in Settings.Default.TransferManager.Split(';').Select(s => s.Trim()))
                {
                    if (String.IsNullOrEmpty(typeName))
                        continue;
                    try
                    {
                        Type type = Type.GetType(typeName);
                        if (type.GetInterface("InvoiceClient.TransferManagement.ITransferManager") != null)
                        {
                            ITransferManager manager = (ITransferManager)type.Assembly.CreateInstance(type.FullName);
                            _ManagerInstance[type] = manager;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
            }

            _InspectorInstance = new Dictionary<Type, ServerInspector>();
            ServerInspector chainedInspector = null;
            if (!String.IsNullOrEmpty(Settings.Default.ServerInspector))
            {
                foreach (String typeName in Settings.Default.ServerInspector.Split(';').Select(s => s.Trim()))
                {
                    if (String.IsNullOrEmpty(typeName))
                        continue;
                    try
                    {
                        Type type = Type.GetType(typeName);
                        if (type.IsSubclassOf(typeof(ServerInspector)))
                        {
                            ServerInspector inspector = (ServerInspector)type.Assembly.CreateInstance(type.FullName);
                            _InspectorInstance[type] = inspector;
                            inspector.ChainedInspector = chainedInspector;
                            chainedInspector = inspector;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
            }

            _MainTabs = new List<Type>();
            if (!String.IsNullOrEmpty(Settings.Default.MainTabs))
            {
                foreach (String typeName in Settings.Default.MainTabs.Split(';').Select(s => s.Trim()))
                {
                    if (String.IsNullOrEmpty(typeName))
                        continue;
                    try
                    {
                        Type type = Type.GetType(typeName);
                        _MainTabs.Add(type);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
            }

        }

        public static IEnumerable<ITransferManager> AllTransferManager
        {
            get
            {
                return _ManagerInstance.Values;
            }
        }

        public static ITransferManager GetTransferManager(Type type)
        {
            return _ManagerInstance[type];
        }

        public static ITransferManager GetTransferManagerForCurrentUI(Type uiType)
        {
            return _ManagerInstance.Values.Where(m => m.UIConfigType == uiType).First();
        }


        public static IEnumerable<ServerInspector> AllServerInspector
        {
            get
            {
                return _InspectorInstance.Values;
            }
        }

        public static IEnumerable<Type> AllMainTabs
        {
            get
            {
                return _MainTabs;
            }
        }

        public static ServerInspector GetServerInspector(Type type)
        {
            return _InspectorInstance[type];
        }

        public static ServerInspector GetServerInspectorForCurrentUI(Type uiType)
        {
            return _InspectorInstance.Values.Where(i => i.UIConfigType == uiType).First();
        }


        public static void StartUp(String fullPath)
        {
            ResetServiceController();
            foreach (var instance in _ManagerInstance.Values)
            {
                instance.PauseAll();
            }

            if (_ServiceController == null || _ServiceController.Status != ServiceControllerStatus.Running)
            {
                foreach (var instance in _ManagerInstance.Values)
                {
                    instance.EnableAll(fullPath);
                }
            }

            foreach (var inspector in _InspectorInstance.Values)
            {
                inspector.StartUp();
            }

        }

        public static void ResetServiceController()
        {
            if (Environment.UserInteractive)
            {
                _ServiceController = ServiceController.GetServices().Where(s => s.ServiceName == Settings.Default.ServiceName).FirstOrDefault();
            }
        }

        public static void ClearServiceController()
        {
            _ServiceController = null;
        }

        public static ServiceController ServiceInstance
        {
            get
            {
                return _ServiceController;
            }
        }

    }

    public interface ITransferManager
    {
        void EnableAll(String fullPath);
        void PauseAll();
        String ReportError();
        void SetRetry();
        Type UIConfigType { get; }
    }
}
