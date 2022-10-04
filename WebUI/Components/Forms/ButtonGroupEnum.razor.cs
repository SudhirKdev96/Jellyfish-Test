using Microsoft.AspNetCore.Components.Forms;
using System;

namespace WebUI.Components.Forms
{
    /// <summary>
    /// Component that turns an entire set of enum values into a button selection group
    /// </summary>
    public partial class ButtonGroupEnum<TEnum> : InputBase<TEnum> where TEnum : struct, Enum { }
}
