using System;
using System.IO;

namespace Common
{
    public abstract class Feature : IDisposable
    {
        private readonly Stream _outputStream;
        private readonly StreamWriter _streamWriter;
        
        public string Name { get; }

        protected Feature(Stream outputStream, string name)
        {
            _outputStream = outputStream;
            _streamWriter = new StreamWriter(_outputStream);

            Name = name;
        }

        public abstract void Run();

        protected void WriteLine(string s)
        {
            _streamWriter.WriteLine(s);
            _streamWriter.Flush();
        }

        public void Dispose()
        {
            _streamWriter.Dispose();
            _outputStream?.Dispose();
        }
    }
}