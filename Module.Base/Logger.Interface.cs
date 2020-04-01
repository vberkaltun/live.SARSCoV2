using System;

namespace live.SARSCoV2.Module.Base
{
    interface ILogger
    {
        void General(string message, bool newLine = true);
        void Informational(string message, bool newLine = true);
        void Initialize(string message, bool newLine = true);
        void Read(string message, bool newLine = true);
        void Write(string message, bool newLine = true);
        void Error(string message, bool newLine = true);
        void Succesfull(string message, bool newLine = true);

        void Print(string message, ConsoleColor color, bool newLine = true);
        void Print(string message, bool newLine = true);

        string ReadMessage();
        char ReadChar();

        void SetVisibleMessage(bool flag = true);
        bool GetVisibleMessage();
    }
}
