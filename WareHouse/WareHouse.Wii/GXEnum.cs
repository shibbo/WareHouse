using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.Wii
{
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
}
