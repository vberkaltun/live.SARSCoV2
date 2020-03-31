using System;

namespace live.SARSCoV2.Module.Base
{
    interface ILogger
    {
        void PrintMessage(string message, bool newLine = true);
        void PrintMessage(string message, ConsoleColor color, bool newLine = true);
        void PrintMessage(string message, Logger.JobType type);

        char ReadChar();
        string ReadMessage();

        bool GetVisibleMessage();
        void SetVisibleMessage(bool flag = true);
    }
}
