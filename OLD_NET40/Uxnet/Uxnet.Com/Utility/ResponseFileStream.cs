using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Utility
{
    public class ResponseFileStream : Stream
    {
        private FileStream _fs;
        protected Stream _base;
        
        public ResponseFileStream(String filePath, Stream baseStream)
        {
            _fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            _base = baseStream;
        }

        public override bool CanRead
        {
            get { return _base.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _base.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _base.CanWrite; }
        }

        public override void Flush()
        {
            _fs.Flush();
            _base.Flush();
        }

        public override long Length
        {
            get { return _base.Length; }
        }

        public override long Position
        {
            get
            {
                return _base.Position;
            }
            set
            {
                _base.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _base.Read(buffer,offset,count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _base.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _base.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _fs.Write(buffer, offset, count);
            _base.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _fs.Flush();
                _fs.Close();
                _fs.Dispose();
            }
        }
    }
}