
using System.Drawing;
using Silk.NET.Windowing;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using WareHouse.windowing;
using ImGuiNET;
using NativeFileDialogSharp;
using WareHouse.ui.widgets;
using System.Numerics;
using System.Runtime.InteropServices;
using WareHouse.util;
using WareHouse.io;
using WareHouse.io.archive;
using WareHouse.Wii.bfres;
using Silk.NET.Maths;
using Silk.NET.Vulkan;

namespace WareHouse.ui
{
    public class MainWindow
    {
        public MainWindow()
        {
            WindowManager.CreateWindow(out mWindow,
             onConfigureIO: () => {
                 unsafe
                 {
                     var io = ImGui.GetIO();

                     var nativeConfig = ImGuiNative.ImFontConfig_ImFontConfig();
                     var iconConfig = ImGuiNative.ImFontConfig_ImFontConfig();

                     //Add a higher horizontal/vertical sample rate for global scaling.
                     nativeConfig->OversampleH = 8;
                     nativeConfig->OversampleV = 8;
                     nativeConfig->RasterizerMultiply = 1f;
                     nativeConfig->GlyphOffset = new Vector2(0);

                     iconConfig->OversampleH = 2;
                     iconConfig->OversampleV = 2;
                     iconConfig->RasterizerMultiply = 1f;
                     iconConfig->GlyphOffset = new Vector2(0);

                     {
                         mDefaultFont = io.Fonts.AddFontFromFileTTF(
                             Path.Combine("res", "Font.ttf"),
                             16, nativeConfig, io.Fonts.GetGlyphRangesJapanese());

                         //other fonts go here and follow the same schema

                         iconConfig->MergeMode = 1;

                         GCHandle rangeHandle = GCHandle.Alloc(new ushort[] { IconUtil.MIN_GLYPH_RANGE, IconUtil.MAX_GLYPH_RANGE, 0 }, GCHandleType.Pinned);
                         try
                         {
                             io.Fonts.AddFontFromFileTTF(
                                 Path.Combine("res", "la-regular-400.ttf"),
                                 16, iconConfig, rangeHandle.AddrOfPinnedObject());

                             io.Fonts.AddFontFromFileTTF(
                                 Path.Combine("res", "la-solid-900.ttf"),
                                 16, iconConfig, rangeHandle.AddrOfPinnedObject());

                             io.Fonts.AddFontFromFileTTF(
                                 Path.Combine("res", "la-brands-400.ttf"),
                                 16, iconConfig, rangeHandle.AddrOfPinnedObject());

                             io.Fonts.Build();
                         }
                         finally
                         {
                             if (rangeHandle.IsAllocated)
                                 rangeHandle.Free();
                         }
                     }
                 }
             });

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

            DrawMainMenu();

            if (mSelectedFile != "")
            {
                /* now that we have our file, let's determine what we do from here */
                string ext = Path.GetExtension(mSelectedFile);
                switch (ext)
                {
                    case ".arc":
                        PlatformUtil.SetPlatform(PlatformUtil.Platform.RVL);
                        mCurrentArchive = new U8Archive(new MemoryFile(File.ReadAllBytes(mSelectedFile)));
                        mShowFileSelection = true;
                        break;
                    case ".brres":
                        PlatformUtil.SetPlatform(PlatformUtil.Platform.RVL);
                        mCurrentModel = new BRRES(new MemoryFile(File.ReadAllBytes(mSelectedFile)));
                        break;
                    case ".szs":
                        PlatformUtil.SetPlatform(PlatformUtil.Platform.RVL);
                        
                        if (FileUtil.IsFileYaz0(mSelectedFile))
                        {
                            byte[] bytes = File.ReadAllBytes(mSelectedFile);
                            Yaz0Archive.Decompress(ref bytes);
                            mCurrentArchive = new U8Archive(new MemoryFile(bytes));
                            mShowFileSelection = true;
                        }

                        break;
                }
            }

            if (mShowFileSelection)
            {
                DrawFileSelect();
            }

            gl.Viewport(mWindow.FramebufferSize);

            controller.Render();
        }

        public void Close()
        {

        }

        private void DrawFileSelect()
        {
            ImGui.Begin("Select File");

            if (mCurrentArchive == null)
            {
                throw new Exception("MainWindow::DrawFileSelect() -- mCurrentArchive is null.");
            }

            string[]? files = mCurrentArchive.GetFiles();
            string[]? nwFiles = PlatformUtil.GetFileExtsForCurrentPlatform();

            if (files == null)
            {
                throw new Exception("MainWindow::DrawFileSelect() -- There are no files in this archive.");
            }

            bool hasNWFiles = false;

            foreach (string file in files)
            {
                string ext = Path.GetExtension(file);

                if (nwFiles.Contains(ext)) {
                    hasNWFiles = true;
                    break;
                }
            }

            if (!hasNWFiles)
            {
                throw new Exception("MainWindow::DrawFileSelect() -- There are no NintendoWare formats is this archive.");
            }

            foreach (string file in files)
            {
                if (ImGui.Selectable(file, true, ImGuiSelectableFlags.AllowDoubleClick)) {
                    switch (PlatformUtil.GetPlatform())
                    {
                        case PlatformUtil.Platform.RVL:
                            switch (Path.GetExtension(file))
                            {
                                case ".brres":
                                    byte[]? fileData = mCurrentArchive.GetFileData(file) ?? throw new Exception("MainWindow::DrawFileSelect() -- File data is null.");
                                    mCurrentModel = new BRRES(new MemoryFile(fileData));
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        private void DrawMainMenu()
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Open File..."))
                    {
                        FileDialog dialog = new FileDialog();
                        if (dialog.ShowDialog("Select File", "brres,arc,szs"))
                        {
                            mSelectedFile = dialog.SelectedFile;
                        }
                    }
                }
            }
        }

        readonly IWindow? mWindow;
        private ImFontPtr mDefaultFont;
        private ImFontPtr mIconFont;
        private string mSelectedFile = "";
        private IArchive? mCurrentArchive = null;
        private IModel? mCurrentModel = null;
        private bool mShowFileSelection = false;
    }
}
