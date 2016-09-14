using System;

namespace Model.Locale
{
    /// <summary>
    /// Naming ���K�n�y�z�C
    /// </summary>
    public class Naming
    {
        private Naming()
        {
            //
            // TODO: �b���[�J�غc�禡���{���X
            //
        }

        public const int INSURER = 5;

        public enum CategoryID
        {
            COMP_SYS = 1,	                            //	1	�t�Τ��q	 sketch_admin.gif
            COMP_WELFARE = 14,
            COMP_E_INVOICE_B2C_SELLER = 15,	            //	2	���	     sketch_seller.gif
            COMP_E_INVOICE_B2C_BUYER = 16,	            //	3	�R��	     sketch_buyer.gif
            COMP_E_INVOICE_GOOGLE_TW = 17,              //  4   Google�x�W
            COMP_ENTERPRISE_GROUP   =   18,
            COMP_VIRTUAL_CHANNEL = 19,
            COMP_INVOICE_AGENT = 20,                    //  5   �o���}�ߥN�z
        }

        public enum B2CCategoryID
        {
            ���a = 15,	                                //	2	���	     sketch_seller.gif
            Google�x�W = 17,                            //  4  
            ���a�o���۰ʰt�� = 19,
            �}�ߵo�����a�N�z = 20,
        }

        public enum RoleID
        {
            ROLE_SYS = 1,	                            //	1	�t�Τ��q	 sketch_admin.gif
            ROLE_SELLER = 51,	                        //	2	���	     sketch_seller.gif
            ROLE_BUYER = 52,	                        //	3	�R��	     sketch_buyer.gif
            ROLE_GUEST = 53,
            ROLE_NETWORKSELLER = 54,                    //  4   ���ʷ~��
            ROLE_GOOGLETW = 55,                         //  5   GOOGLE�x�W
            ���Φ��� = 61,
            �۹���~�H = 62,
            ���Φ���_�۰ʶ}�߱��� = 63
        }

        public enum RoleQueryDefinition
        {
            ���x�޲z�� = 1,	                            //	1	�t�Τ��q	 sketch_admin.gif
            ��� = 51,	                                //	2	���	     sketch_seller.gif
            �R�� = 52,	                                //	3	�R��	     sketch_buyer.gif
            �X�� = 53,
            ���ʷ~�� = 54,                              //  4   ���ʷ~��
            Google�x�W = 55,                            //  5   GOOGLE�x�W
            ���Φ��� = 61,
            �۹���~�H = 62,
            ���Φ���_�۰ʶ}�߱��� = 63
        }

        public enum DocumentTypeDefinition
        {
            E_Invoice = 10,                             //	�q�l�o��
            E_Allowance = 11,                           //	�q�l�o�������ҩ�
            E_InvoiceCancellation = 12,                 //  �@�o�q�l�o��
            E_InvoiceReturn = 13,                       //	�q�l�o���h�^
            E_AllowanceCancellation = 14,               //	�@�o�q�l�o�������ҩ�
            E_InvoiceVoid = 15,	                        //  �q�l�o�����P
            E_Receipt = 16,                             //  ����
            E_ReceiptCancellation = 17,                 //  �@�o����
            PurchaseOrder = 100,                        //  ���ʳ�
            WarehouseWarrant = 101,                     //  �J�w��
            PurchaseOrderReturned = 102,                //	���ʰh�f��
            BuyerOrder = 103,                           //	�q��
            OrderExchangeGoods = 104,                   //	���f��
            OrderGoodsReturned = 105                    //	�h�f��
        }

        public enum B2BInvoiceDocumentTypeDefinition
        {
            �q�l�o�� = 10,                              //	�q�l�o��
            �o������ = 11,                              //	�q�l�o�������ҩ�
            �@�o�o�� = 12,                              //  �@�o�q�l�o��
            �@�o���� = 14,
            ���� = 16,
            �@�o���� = 17
        }

        public enum MemberStatusDefinition
        {
            Mark_To_Delete = 1101,                      //1101	���O����	    ���O����
            Wait_For_Check = 1102,                      //1102	���ݦ^�нT�{	���ݦ^�нT�{
            Checked = 1103                              //1103	�H���w�T�{	    �H���w�T�{
        }

        public enum CACatalogDefinition
        {
            ñ������ = 0,                               //ñ������
            �r�y���@ = 1,                               //�r�y���@
            �}�ߵo�� = 2,
            �}�ߧ@�o�o�� = 3,
            �}�ߧ����� = 4,
            �}�ߧ@�o������ = 5,
            �U���j���x�o�� = 6,
            �U���j���x�@�o�o�� = 7,
            �U���j���x���� = 8,
            �U���j���x�@�o���� = 9,
            UXGW�W�Ǹ�� = 10,
            ���v��ƤU�� = 11,
            �����o��=12,
            ���������� = 13,
            �����@�o�o�� = 14,
            �����@�o������ = 15,
            �C�L�o�� = 16,
            �C�L������ = 17,
            �C�L�@�o�o�� = 18,
            �C�L�@�o������ = 19,
            �]�wñ������ = 20,
            UXGW�۰ʱ��� = 21,
            ���x�۰ʱ��� = 22,
            ���x�۰ʶ}�� = 23,
            UXGW�W�Ǫ����� = 24,
            �}�ߦ��� = 25,
            �}�ߧ@�o���� = 26,
            �������� = 27,
            �����@�o���� = 28,
            �h�^�o�� = 29,
            �h�^�@�o�o�� = 30,
            �h�^������ = 31,
            �h�^�@�o������ = 32,
            �h�^���� = 33,
            ���P�o�� = 34,
            ���P�@�o�o�� = 35,
            ���P������ = 36,
            ���P�@�o������ = 37,
            ���P���� = 38,
            UXGW�۰ʶ}�� = 39,
            UXGW�U�����    = 40,
            UXGW�W�ǵo���h�^ = 41,
            UXGW�W�ǵo�����P = 42,
            UXGW�W�ǧ�����h�^ = 43,
            UXGW�W�ǧ�������P = 44,
            UXGW�W�ǧ@�o�o���h�^ = 45,
            UXGW�W�ǧ@�o�o�����P = 46,
            UXGW�W�ǧ@�o������h�^ = 47,
            UXGW�W�ǧ@�o��������P = 48,
            UXGW���^�۹���~�H������ = 49,
            UXGW�����۹���~�H�h�^�ӽ� = 50
        }

        public enum B2BCACatalogQueryDefinition
        {
            �r�y���@ = 1,                                //�r�y���@
            �}�ߵo�� = 2,
            �}�ߧ@�o�o�� = 3,
            �}�ߧ����� = 4,
            �}�ߧ@�o������ = 5,
            �����o�� = 12,
            ���������� = 13,
            �����@�o�o�� = 14,
            �����@�o������ = 15,
            �C�L�o�� = 16,
            �C�L������ = 17,
            �C�L�@�o�o�� = 18,
            �C�L�@�o������ = 19,
            �]�wñ������ = 20,
            ���x�۰ʱ��� = 22,
            ���x�۰ʶ}�� = 23,
            �h�^�o�� = 29,
            �h�^�@�o�o�� = 30,
            �h�^������ = 31,
            �h�^�@�o������ = 32,
            �h�^���� = 33,
            ���P�o�� = 34,
            ���P�@�o�o�� = 35,
            ���P������ = 36,
            ���P�@�o������ = 37,
            ���P���� = 38,
            UXGW�W�Ǹ�� = 10,
            UXGW�۰ʱ��� = 21,
            UXGW�W�Ǫ����� = 24,
            UXGW�U��PDF = 40,
            UXGW�W�ǵo���h�^ = 41,
            UXGW�W�ǵo�����P = 42,
            UXGW�W�ǧ�����h�^ = 43,
            UXGW�W�ǧ�������P = 44,
            UXGW�W�ǧ@�o�o���h�^ = 45,
            UXGW�W�ǧ@�o�o�����P = 46,
            UXGW�W�ǧ@�o������h�^ = 47,
            UXGW�W�ǧ@�o��������P = 48,
            UXGW�����o�� = 49,
            UXGW�����@�o�o�� = 50,
            UXGW���������� = 51,
            UXGW�����@�o������ = 52,
            UXGW�۹���~�H�������o�� = 53,
            UXGW�۹���~�H�������@�o�o�� = 54,
            UXGW�۹���~�H������������ = 55,
            UXGW�۹���~�H�������@�o������ = 56
        }

        public enum eIVOUXCACatalogQueryDefinition
        {
            UXGW�W�Ǹ�� = 10,
            �]�wñ������ = 20,
        }

        public enum UploadStatusDefinition
        {
            ���ݶפJ = 0,
            ��ƿ��~ = 1,
            �פJ���\ = 2,
            �פJ���� = 3
        }

        public enum DocumentStepDefinition
        {
            �w�� = 1200,
            �ݼf�� = 1201,
            �w�}�� = 1202,
            �w�h�^ = 1203,
            �w�R�� = 1204
        }

        public enum BuyerOrderTypeDefinition
        {
            �@����O�� = 1,
            ������ = 2,
            ���� = 3
        }

        public enum DataItemSource
        {
            FromViewState = 0,
            FromDB = 1,
            FromPreviousPage = 2
        }

        public enum DataItemStatus
        {
            Unchanged = 0,
            Modified = 1
        }

        public enum B2BInvoiceStepDefinition
        {
            �ݱ��� = 1301,                               //	�ݱ���           
            �ݶ}�� = 1302,                               //	�ݶ}��
            �ݶǰe = 1303,                               //	�ݶǰe��IFS-EIVO
            �w�ǰe = 1304,	                             //	�w�ǰe��IFS-EIVO
            �w���� = 1305,
            �w�}�� = 1306,
            �w���P = 1307,
            �ݧ妸�ǰe = 1308,
            �ӽаh�^ = 1309,
            �w�@�o = 1310,
            Xml�ݶǿ� = 1311,
            PDF�ݶǿ� = 1312,
            ��������ƫݳq�� = 1313,
            ��������ƫݶǿ� = 1314,
            ���P�ӽЫݶ}�� = 1315,
            �h�^�ӽЫݶ}�� = 1316
        }

        public enum B2BInvoiceQueryStepDefinition
        { 
            �ݱ��� = 1301,                               //	�ݱ���
            �ݶ}�� = 1302,                               //	�ݶ}��
            �w�}�� = 1306,
            �w���P = 1307,
            �w���� = 1308,                               //��W�� "�ݧ妸�ǰe",�]��ܻݭn�� "�w����"
            �ӽаh�^ = 1309
        }

        public enum CenterInvoiceReturnStepDefinition
        {
            �ݱ��� = 1301,                                //	�ݱ���
            �w���� = 1308,                                //��W�� "�ݧ妸�ǰe",�]��ܻݭn�� "�w����"
            �ӽаh�^ = 1309
        }

        public enum InvoiceCenterBusinessType
        { 
            �P��  =   1,
            �i��  =   2
        }

        public enum CounterpartBusinessQueryType
        {
            //�P�� = 1,
            �i�� = 2
        }        

        public enum InvoiceCenterBusinessQueryType
        {
            �P�� = 1,
            �i�� = 2
        }

        public enum MessageTypeDefinition
        {
            �o���}�߳q�� = 1,
            �o�������q�� = 2,
            �t�ΰT���q�� = 3,
            �ȪA�T���q�� = 4
        }

        public enum B2CInvoiceLevelDefinition
        {
            �w�}�� = 0,
            �ݶǰe = 1,
            �w���P = 6
        }

        public enum InvoiceTypeDefinition
        {
            �T�p�� = 1,
            �G�p�� = 2,
            �G�p�����Ⱦ� = 3,
            �S�ص|�B = 4,
            �q�l�p��� = 5,
            �T�p�����Ⱦ� = 6,
            �@��|�B�p�⤧�q�l�o�� = 7,
            �S�ص|�B�p�⤧�q�l�o�� = 8,
        }

        public enum InvoiceTypeFormat
        {
            �T�p��, �q�l�p��� = 21,
            �G�p�� = 32,
            �G�p�����Ⱦ� = 22,
            �T�p�����Ⱦ� = 25,
            �S�ص|�B = 37,
            �@��|�B�p�⤧�q�l�o�� = 25,
        }

        public enum TaxTypeDefinition
        {
            ���| = 1,
            �s�|�v = 2,
            �K�| = 3,
            �S�ص|�v = 4,
            �V�X�|�v = 9
        }

        public enum InvoiceDeliveryStatus
        {
            �ݶǰe = 1303,                               //	�ݶǰe��IFS-EIVO
            �w�ǰe = 1304,	                             //	�w�ǰe��IFS-EIVO
            �ӽаh�^ = 1309
        }
        public enum ChannelIDType
        {
            FromWeb = 0,
            FromGW=1
        }

        public enum DataResultMode
        {
            Display = 0,
            Print = 1,
            Download = 2
        }

    }
}
