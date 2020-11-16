using System.Collections;

namespace mini_java_compiler.Parse
{

    public class TokenStack
    {
        protected Stack stack;

        public TokenStack()
        {
            stack = new Stack();
        }

        public virtual void Clear()
        {
            stack.Clear();
        }

        public virtual Token Peek()
        {
            return (Token)stack.Peek();
        }

        public virtual Token Pop()
        {
            return (Token)stack.Pop();
        }

        public virtual void Push(Token token)
        {
            stack.Push(token);
        }

        public virtual int Count { get { return stack.Count; } }


    }
}
