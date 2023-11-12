
using System.Drawing;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace WareHouse.ui
{
    public class MainWindow
    {
        public MainWindow()
        {
            GL? gl = null;
            IInputContext? inputContext = null;

            mWindow = Window.Create(WindowOptions.Default);

            mWindow.Load += () =>
            {
                mController = new ImGuiController(
                    gl = mWindow.CreateOpenGL(), // load OpenGL
                    mWindow, // pass in our window
                    inputContext = mWindow.CreateInput() // create an input context
                );
            };

            mWindow.FramebufferResize += s =>
            {
                // Adjust the viewport to the new window size
                gl?.Viewport(s);
            };

            mWindow.Render += delta =>
            {
                // Make sure ImGui is up-to-date
                mController?.Update((float)delta);

                // This is where you'll do any rendering beneath the ImGui context
                // Here, we just have a blank screen.
                gl?.ClearColor(Color.FromArgb(255, (int)(.45f * 255), (int)(.55f * 255), (int)(.60f * 255)));
                gl?.Clear((uint)ClearBufferMask.ColorBufferBit);

                // This is where you'll do all of your ImGUi rendering
                // Here, we're just showing the ImGui built-in demo window.
                ImGuiNET.ImGui.ShowDemoWindow();

                // Make sure ImGui renders too!
                mController?.Render();
            };

            mWindow.Closing += () =>
            {
                // Dispose our controller first
                mController?.Dispose();

                // Dispose the input context
                inputContext?.Dispose();

                // Unload OpenGL
                gl?.Dispose();
            };

            mWindow.Run();
            mWindow.Dispose();
        }

        ImGuiController? mController;
        IWindow? mWindow;
    }
}
