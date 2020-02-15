using System;
using System.Threading;

namespace tplStuff
{
    class SimpleWaitPulse {
        static Foo foo = new Foo();

        static void Main() {
            new Thread(foo.third).Start();
            new Thread(foo.first).Start();
            new Thread(foo.second).Start();
        }
    }

    class Foo {
        object locker = new object();
        int num = 0;

        public void first() {
            Init(()=>System.Console.WriteLine("first"));
        }
        public void second() {
            Wait(1,()=>System.Console.WriteLine("second"));
        }
        public void third() {
            Wait(2,()=>System.Console.WriteLine("third"));
        }

        void Init(Action doit) {
            lock (locker) {
                doit();
                num = 1;
                Monitor.Pulse(locker);
            }
        }

        void Wait(int n, Action doit) {
            lock (locker) {
                while (num != n) {
                    Monitor.Wait(locker);
                }
                doit();
                num = n+1;
                Monitor.Pulse(locker);
            } 
        }
    }
}




