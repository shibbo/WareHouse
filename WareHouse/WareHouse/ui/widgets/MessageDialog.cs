using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using ImGuiNET;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WareHouse.ui.widgets
{
    public class MessageBox
    {
        public enum MessageBoxType
        {
            YesNo = 0,
            Ok = 1
        }

        public enum MessageBoxResult
        {
            Waiting = -1,
            Ok = 0,
            No = 1,
            Yes = 2,
            Closed = 3
        }

        public MessageBox(MessageBoxType type)
        {
            mType = type;
        }

        public MessageBoxResult Show(string header, string message)
        {
            MessageBoxResult res = MessageBoxResult.Waiting;

            bool needsClose = true;
            bool status = ImGui.Begin(header, ref needsClose);
            ImGui.Text(message);

            switch (mType)
            {
                case MessageBoxType.Ok:
                    if (ImGui.Button("OK"))
                    {
                        res = MessageBoxResult.Ok;
                    }
                    break;
                case MessageBoxType.YesNo:
                    if (ImGui.Button("Yes"))
                    {
                        res = MessageBoxResult.Yes;
                    }

                    ImGui.SameLine();

                    if (ImGui.Button("No"))
                    {
                        res = MessageBoxResult.No;
                    }
                    break;
            }

            if (!needsClose)
            {
                res = MessageBoxResult.Closed;
            }

            if (status)
            {
                ImGui.End();
            }

            return res;
        }

        MessageBoxType mType;
    }
}
