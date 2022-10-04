using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace WebUI.Components.Forms
{
    public class InputCurrency<TValue> : InputNumber<TValue>
    {
        private bool _isFocused;

        /// <summary>
        /// An additional event to invoke after the bound value changes
        /// </summary>
        [Parameter]
        public EventCallback<string?> OnChange { get; set; }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            // replicates the render tree building of InputNumber, except uses type=text (instead of number),
            // and adds state events on focus in and out
            int i = 0;
            builder.OpenElement(i++, "input");
            builder.AddMultipleAttributes(i++, AdditionalAttributes);
            builder.AddAttribute(i++, "type", "text");
            builder.AddAttribute(i++, "class", CssClass);
            builder.AddAttribute(i++, "value", BindConverter.FormatValue(CurrentValueAsString));
            builder.AddAttribute(i++, "onchange", EventCallback.Factory.CreateBinder<string?>(this, __value => FireChange(__value), CurrentValueAsString));
            builder.AddAttribute(i++, "onfocus", EventCallback.Factory.Create<FocusEventArgs>(this, _ => _isFocused = true));
            builder.AddAttribute(i++, "onfocusout", EventCallback.Factory.Create<FocusEventArgs>(this, _ => _isFocused = false));
            builder.CloseElement();
        }

        protected void FireChange(string? value)
        {
            CurrentValueAsString = value;
            OnChange.InvokeAsync();
        }

        /// <inheritdoc />
        protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string validationErrorMessage)
        {
            // strips dollar signs and commas when validating input, so that a user could copy-paste a money value
            // with those in it and it would still be accepted
            string s = value?.Replace("$", "").Replace(",", "");
            return base.TryParseValueFromString(s, out result, out validationErrorMessage);
        }

        /// <inheritdoc />
        protected override string FormatValueAsString(TValue? value)
        {
            // displays a fully formatted curency value when focus isn't in the input,
            // but when the user clicks into it, shows a plain number for easier editing
            return _isFocused ? $"{value}" : $"{value:c}";
        }
    }
}
