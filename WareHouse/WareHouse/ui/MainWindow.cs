
using System.Drawing;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using WareHouse.windowing;
using ImGuiNET;

namespace WareHouse.ui
{
    public class MainWindow
    {
        public MainWindow()
        {
            WindowManager.CreateWindow(out mWindow);

            mWindow.Load += () => WindowManager.RegisterRenderDelegate(mWindow, Render);
            mWindow.Closing += Close;

            mWindow.Run();
            mWindow.Dispose();
        }

        public void Render(GL gl, double delta, ImGuiController controller)
        {
            if (mWindow == null)
            {
                throw new Exception("MainWindow::Render() -- mWindow is null.");
            }

            gl.Viewport(mWindow.FramebufferSize);

            gl.ClearColor(.45f, .55f, .60f, 1f);
            gl.Clear((uint)ClearBufferMask.ColorBufferBit);

            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            ImGui.DockSpaceOverViewport();

            gl.Viewport(mWindow.FramebufferSize);

            controller.Render();
        }

        public void Close()
        {

        }

        ImGuiController? mController;
        IWindow? mWindow;
    }
}
