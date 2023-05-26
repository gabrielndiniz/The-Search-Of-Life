//this is an interface between ActionScheduler, Mover and Fighter for now
namespace BeatEmUp.Controller
{
    public interface IAction
    {
        void Cancel();
    }
}