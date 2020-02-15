using System;
using System.Threading;

namespace tplStuff
{
    class PrintInOrder {
        static Foo foo = new Foo();

        static void Main() {
            new Thread(foo.third).Start();
            new Thread(foo.first).Start();
            new Thread(foo.second).Start();
        }
    }

    class Foo : StateMachine {
        public void first() {
            Init(
                doit:()=>System.Console.WriteLine("first"), 
                next:1);
        }

        public void second() {
            OnNext(
                state:1, 
                doit:()=>System.Console.WriteLine("second"), 
                next:2);
        }

        public void third() {
            OnNext(
                state:2, 
                doit:()=>System.Console.WriteLine("third"), 
                next:3);
        }
    }

    abstract class StateMachine {
        object locker = new object();
        int currState = 0;

        protected void Init(Action doit, int next) {
            lock (locker) {
                doit();
                TransitionTo(next);
            }
        }

        protected void OnNext(int state, Action doit, int next) {
            lock (locker) {
                WaitOn(state);
                doit();
                TransitionTo(next);
            } 
        }

        void WaitOn(int wait) {
            while (currState != wait) {
                Monitor.Wait(locker);
                Monitor.Pulse(locker);
            }
        }

        void TransitionTo(int next) {
            currState = next;
            Monitor.Pulse(locker);
        }
    } 
}




