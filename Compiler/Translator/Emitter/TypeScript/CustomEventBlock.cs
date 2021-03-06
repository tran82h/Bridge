﻿using Bridge.Contract;
using ICSharpCode.NRefactory.CSharp;

namespace Bridge.Translator.TypeScript
{
    public class CustomEventBlock : TypeScriptBlock
    {
        public CustomEventBlock(IEmitter emitter, CustomEventDeclaration customEventDeclaration)
            : base(emitter, customEventDeclaration)
        {
            this.Emitter = emitter;
            this.CustomEventDeclaration = customEventDeclaration;
        }

        public CustomEventDeclaration CustomEventDeclaration
        {
            get;
            set;
        }

        protected override void DoEmit()
        {
            this.EmitPropertyMethod(this.CustomEventDeclaration, this.CustomEventDeclaration.AddAccessor, false);
            this.EmitPropertyMethod(this.CustomEventDeclaration, this.CustomEventDeclaration.RemoveAccessor, true);
        }

        protected virtual void EmitPropertyMethod(CustomEventDeclaration customEventDeclaration, Accessor accessor, bool remover)
        {
            if (!accessor.IsNull && this.Emitter.GetInline(accessor) == null)
            {
                XmlToJsDoc.EmitComment(this, customEventDeclaration);
                this.Write(Helpers.GetEventRef(customEventDeclaration, this.Emitter, remover, false, false));
                this.WriteOpenParentheses();
                this.Write("value");
                this.WriteColon();
                var retType = BridgeTypes.ToTypeScriptName(customEventDeclaration.ReturnType, this.Emitter);
                this.Write(retType);
                this.WriteCloseParentheses();
                this.WriteColon();
                this.Write("void");

                this.WriteSemiColon();
                this.WriteNewLine();
            }
        }
    }
}