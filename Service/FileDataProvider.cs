using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;
using Service.Model;

namespace Service
{
    public class FileDataProvider : 
        IDisposable
    {
        private readonly ILogger<FileDataProvider> _logger;
        private readonly Operations _operations;
        private StreamReader _fileStream;
        private FileStream _file;
        
        private bool _isDisposed = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="validate">input param validate </param>
        /// <param name="operations"> </param>
        /// <param name="fileName"> </param>
        public FileDataProvider(
            ILogger<FileDataProvider> logger,
            Operations operations,
            string fileName)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _operations = operations ?? throw new ArgumentNullException(nameof(operations));
            _file = new FileStream(fileName, FileMode.Open);
            _fileStream = new StreamReader(_file);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposed)
        {
            if (disposed)
            {
                _fileStream.Dispose();
                _file.Dispose();
                _fileStream = null;
                _file = null;
            }

            _isDisposed = true;
        }
        
        public Tree GetTree()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(FileDataProvider));
            }

            string mask = "public static void method(";
            var symbol = _fileStream.Read();
            var buffer = new StringBuilder();
            while (symbol >= 0 )
            {
                if (buffer.ToString().Equals(mask))
                {
                    break;
                }
                
                if (buffer.Length >= mask.Length)
                {
                    buffer.Remove(0, 1);
                }

                buffer.Append((char)symbol);
                symbol = _fileStream.Read();
            }

            while (symbol > 0 && symbol != '{')
            {
                symbol = _fileStream.Read();
            }

            return _operations.GetOperator(_fileStream);
        }
    }
}