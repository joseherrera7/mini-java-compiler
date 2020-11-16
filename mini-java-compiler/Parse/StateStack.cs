using mini_java_compiler.Parse.lalr;
using System.Collections;


namespace mini_java_compiler.Parse
{

    public class StateStack
    {
        protected Stack stack;
        public StateStack()
        {
            stack = new Stack();
        }
        public virtual void Clear()
        {
            stack.Clear();
        }
        public virtual State Peek()
        {
            return (State)stack.Peek();
        }
        public virtual State Pop()
        {
            return (State)stack.Pop();
        }
        public virtual void Push(State state)
        {
            stack.Push(state);
        }
        public virtual int Count { get { return stack.Count; } }


    }
}
