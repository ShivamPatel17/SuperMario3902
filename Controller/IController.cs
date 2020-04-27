
namespace TechSupportMario
{
    interface IController
    {
        void Update();
        void ClearDictionary();
        void FullClear();
        void Add(int key, ICommand command);
        //we can add a Add(int, Command) method if we want
    }
}
