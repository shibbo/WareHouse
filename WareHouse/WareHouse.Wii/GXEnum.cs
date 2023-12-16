using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.Wii
{
    public enum GXCullMode
    {
        GX_CULL_NONE,
        GX_CULL_FRONT,
        GX_CULL_BACK,
        GX_CULL_ALL
    }
    public enum GXTexFmt
    {
        GX_TF_I4 = 0x0,
        GX_TF_I8 = 0x1,
        GX_TF_IA4 = 0x2,
        GX_TF_IA8 = 0x3,
        GX_TF_RGB565 = 0x4,
        GX_TF_RGB5A3 = 0x5,
        GX_TF_RGBA8 = 0x6,
        GX_TF_CMPR = 0xE,
    }
    public enum GXDistAttnFn
    {
        GX_DA_OFF,
        GX_DA_GENTLE,
        GX_DA_MEDIUM,
        GX_DA_STEEP
    }

    public enum GXSpotFn
    {
        GX_SP_OFF,
        GX_SP_FLAT,
        GX_SP_COS,
        GX_SP_COS2,
        GX_SP_SHARP,
        GX_SP_RING1,
        GX_SP_RING2
    }

    public enum GXProjectionType
    {
        GX_PERSPECTIVE,
        GX_ORTHOGRAPHIC
    }

    public enum GXFogType
    {
        GX_FOG_NONE = 0x00,
        GX_FOG_PERSP_LIN = 0x02,
        GX_FOG_PERSP_EXP = 0x04,
        GX_FOG_PERSP_EXP2 = 0x05,
        GX_FOG_PERSP_REVEXP = 0x06,
        GX_FOG_PERSP_REVEXP2 = 0x07,
        GX_FOG_ORTHO_LIN = 0x0A,
        GX_FOG_ORTHO_EXP = 0x0C,
        GX_FOG_ORTHO_EXP2 = 0x0D,
        GX_FOG_ORTHO_REVEXP = 0x0E,
        GX_FOG_ORTHO_REVEXP2 = 0x0F,
        GX_FOG_LIN = GX_FOG_PERSP_LIN,
        GX_FOG_EXP = GX_FOG_PERSP_EXP,
        GX_FOG_EXP2 = GX_FOG_PERSP_EXP2,
        GX_FOG_REVEXP = GX_FOG_PERSP_REVEXP,
        GX_FOG_REVEXP2 = GX_FOG_PERSP_REVEXP2
    }

    public enum GXCompCnt
    {
        GX_POS_XY = 0,
        GX_POS_XYZ = 1,
    }

    public enum GXCompType
    {
        GX_U8 = 0,
        GX_S8 = 1,
        GX_U16 = 2,
        GX_S16 = 3,
        GX_F32 = 4,
    }

    public enum GXTexMapID
    {
        GX_TEXMAP0 = 0,
        GX_TEXMAP1 = 1,
        GX_TEXMAP2 = 2,
        GX_TEXMAP3 = 3,
        GX_TEXMAP4 = 4,
        GX_TEXMAP5 = 5,
        GX_TEXMAP6 = 6,
        GX_TEXMAP7 = 7
    }

    public enum GXTlut
    {
        GX_TLUT0 = 0,
        GX_TLUT1 = 1,
        GX_TLUT2 = 2,
        GX_TLUT3 = 3,
        GX_TLUT4 = 4,
        GX_TLUT5 = 5,
        GX_TLUT6 = 6,
        GX_TLUT7 = 7,
        GX_TLUT8 = 8,
        GX_TLUT9 = 9,
        GX_TLUT10 = 10,
        GX_TLUT11 = 11,
        GX_TLUT12 = 12,
        GX_TLUT13 = 13,
        GX_TLUT14 = 14,
        GX_TLUT15 = 15,
        GX_BIGTLUT0 = 16,
        GX_BIGTLUT1 = 17,
        GX_BIGTLUT2 = 18,
        GX_BIGTLUT3 = 19
    }
    public enum GXTexWrapMode
    {
        GX_CLAMP = 0,
        GX_REPEAT = 1,
        GX_MIRROR = 2
    }

    public enum GXTexFilter
    {
        GX_NEAR = 0,
        GX_LINEAR = 1,
        GX_NEAR_MIP_NEAR = 2,
        GX_LIN_MIP_NEAR = 3,
        GX_NEAR_MIP_LIN = 4,
        GX_LIN_MIP_LIN = 5
    }

    public enum GXAnisotropy
    {
        GX_ANISO_1 = 0,
        GX_ANISO_2 = 1,
        GX_ANISO_4 = 2
    }
}
