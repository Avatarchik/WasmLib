using WasmLib.FileFormat;
using WasmLib.FileFormat.Instructions;
using WasmLib.Utils;

namespace WasmLib.Decompilation.Intermediate.Instructions
{
    public class MemorySizeInstruction : IntermediateInstruction
    {
        public OperationKind Operation { get; }

        public override ValueKind[] PopTypes => Operation == OperationKind.Grow ? new[] {ValueKind.I32} : new ValueKind[0];
        public override ValueKind[] PushTypes => new[] {ValueKind.I32};

        public override StateKind ModifiesState => Operation == OperationKind.Grow ? StateKind.Memory : StateKind.None;
        public override StateKind ReadsState => Operation == OperationKind.Size ? StateKind.Memory : StateKind.None;

        public MemorySizeInstruction(in Instruction instruction)
        {
            Operation = instruction.OpCode switch {
                OpCode.MemorySize => OperationKind.Size,
                OpCode.MemoryGrow => OperationKind.Grow,
                _ => throw new WrongInstructionPassedException(instruction, nameof(MemorySizeInstruction)),
            };
        }

        public override string OperationStringFormat => Operation == OperationKind.Size ? "MEMORY.SIZE / PAGE_SIZE" : "MEMORY.GROW({0} * PAGE_SIZE)";

        public override string ToString() => Operation.ToString();

        public enum OperationKind
        {
            Size,
            Grow,
        }
    }
}