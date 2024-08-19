using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouse.io;

namespace WareHouse.Wii.brlyt
{
    public class BRLYT : ILayout
    {
        public BRLYT(MemoryFile file)
        {
            string magic = file.ReadString(4);

            if (magic != "RLYT")
            {
                throw new Exception("BRLYT::BRLYT() -- Invalid magic.");
            }

            mBOM = file.ReadUInt16();

            if (mBOM != 0xFEFF)
            {
                throw new Exception("BRLYT::BRLYT() -- BOM is not set to big endian");
            }

            mVersion = file.ReadUInt16();
            file.Skip(6);
            mBlockCount = file.ReadUInt16();

            /* layout info */
            mLayout = new(file);

            Pane? previous = null;
            Pane? parent = null;

            Group? previousGroup = null;
            Group? parentGroup = null;

            for (int i = 0; i < mBlockCount - 1; i++)
            {
                string section = file.ReadString(4);

                switch (section)
                {
                    case "txl1":
                        ReadTXL1(file);
                        break;
                    case "fnl1":
                        ReadFNL1(file);
                        break;
                    case "mat1":
                        ReadMAT1(file);
                        break;
                    case "pan1":
                        Pane pane = new(file);

                        if (mRootPane == null)
                        {
                            mRootPane = pane;
                        }

                        if (parent != null)
                        {
                            parent.AddChild(pane);
                            pane.SetParent(parent);
                        }

                        previous = pane;
                        break;

                    case "bnd1":
                        Bound bound = new(file);

                        if (parent != null)
                        {
                            parent.AddChild(bound);
                            bound.SetParent(parent);
                        }

                        previous = bound;
                        break;

                    case "txt1":
                        TextBox textbox = new(file);

                        if (parent != null)
                        {
                            parent.AddChild(textbox);
                            textbox.SetParent(parent);
                        }

                        previous = textbox;
                        break;

                    case "pic1":
                        Picture picture = new(file);

                        if (parent != null)
                        {
                            parent.AddChild(picture);
                            picture.SetParent(parent);
                        }

                        previous = picture;
                        break;

                    case "usd1":
                        throw new Exception("user data spotted?");

                    case "wnd1":
                        Window window = new(file);

                        if (parent != null)
                        {
                            parent.AddChild(window);
                            window.SetParent(parent);
                        }

                        previous = window;
                        break;

                    case "grp1":
                        Group group = new(file);

                        if (mRootGroup == null)
                        {
                            mRootGroup = group;
                        }

                        if (parentGroup != null)
                        {
                            parentGroup.AddChild(group);
                            group.SetParent(parentGroup);
                        }

                        previousGroup = group;
                        break;

                    case "grs1":
                        if (previousGroup != null)
                        {
                            parentGroup = previousGroup;
                        }
                        file.Skip(4);
                        break;

                    case "gre1":
                        previousGroup = parentGroup;
                        parentGroup = previousGroup.GetParent();
                        break;

                    case "pas1":
                        if (previous != null)
                        {
                            parent = previous;
                        }

                        file.Skip(4);
                        break;

                    case "pae1":
                        previous = parent;
                        parent = previous.GetParent();
                        file.Skip(4);
                        break;
                }
            }
        }

        private void ReadTXL1(MemoryFile file)
        {
            int startPos = file.Position() - 4;
            int size = file.ReadInt32();
            ushort num = file.ReadUInt16();
            file.Skip(2);
            int basePos = file.Position();

            for (int i = 0; i < num; i++)
            {
                long loc = file.ReadInt32() + basePos;
                mTextureList.Add(file.ReadStringAtNT((int)loc));
                file.Skip(4);
            }

            file.Seek(startPos + size);
        }

        private void ReadFNL1(MemoryFile file)
        {
            int startPos = file.Position() - 4;
            int size = file.ReadInt32();
            ushort num = file.ReadUInt16();
            file.Skip(2);
            int basePos = file.Position();

            for (int i = 0; i < num; i++)
            {
                long loc = file.ReadInt32() + basePos;
                mFontList.Add(file.ReadStringAtNT((int)loc));
                file.Skip(4);
            }

            file.Seek(startPos + size);
        }

        private void ReadMAT1(MemoryFile file)
        {
            int startPos = file.Position() - 4;
            int size = file.ReadInt32();
            ushort materialCount = file.ReadUInt16();
            file.Skip(2);

            int arrStart = file.Position();

            for (ushort i = 0; i < materialCount; i++)
            {
                // read our offset and save our position so we know where to jump back to, to read the next entry
                int offs = file.ReadInt32();
                int pos = file.Position();
                file.Seek(startPos + offs);
                mMaterials.Add(new(file));
                file.Seek(pos);
            }

            file.Seek(startPos + size);
        }

        ushort mBOM;
        ushort mVersion;
        ushort mBlockCount;
        public Layout mLayout;
        Pane? mRootPane = null;
        Group? mRootGroup = null;
        List<string> mTextureList = new();
        List<string> mFontList = new();
        List<Material> mMaterials = new();
    }

    public class Layout
    {
        public Layout(MemoryFile file)
        {
            if (file.ReadString(4) != "lyt1")
            {
                throw new Exception("Layout::Layout() -- Invalid magic.");
            }

            file.Skip(4);
            mOriginType = file.ReadByte();
            file.Skip(3);
            mWidth = file.ReadSingle();
            mHeight = file.ReadSingle();
        }

        public byte mOriginType;
        public float mWidth;
        public float mHeight;
    }
}
