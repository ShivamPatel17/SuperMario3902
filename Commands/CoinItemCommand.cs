using TechSupportMario.Entity.Block;
using TechSupportMario.Entity.Items;

namespace TechSupportMario.Commands
{
    class CoinItemCommand : ICommand
    {
        private readonly IBlock Question;
        public CoinItemCommand(IBlock question)
        {
            Question = question;
        }
        public void Execute()
        {
            ItemFactory.Instance.QuestionCoin().BounceFromBlock(Question.Position);
        }
    }
}
