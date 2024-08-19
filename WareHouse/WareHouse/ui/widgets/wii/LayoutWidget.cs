using Silk.NET.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImGuiNET;
using WareHouse.Wii.brlyt;

namespace WareHouse.ui.widgets.wii
{
    public static class LayoutWidget
    {
        public static void DrawUI(GL gl, string fileName, BRLYT? layout)
        {
            if (ImGui.Begin(fileName))
            {
                ImGui.Text("Layout Information");
                ImGui.Separator();

                if (ImGui.BeginTable("layoutInfo", 2, ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.Resizable))
                {
                    ImGui.TableSetupColumn("Property");
                    ImGui.TableSetupColumn("Value");
                    ImGui.TableHeadersRow();

                    ImGui.PushID(0);
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);

                    ImGui.Text("Width:");
                    ImGui.TableNextColumn();

                    ImGui.InputFloat("##layoutWidth", ref layout.mLayout.mWidth);

                    ImGui.PushID(1);
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);

                    ImGui.Text("Height:");
                    ImGui.TableNextColumn();

                    ImGui.InputFloat("##layoutHeight", ref layout.mLayout.mHeight);

                    ImGui.EndTable();
                }

                ImGui.End();
            }
        }
    }
}
