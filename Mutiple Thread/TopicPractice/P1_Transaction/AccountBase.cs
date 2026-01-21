using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopicPractice.P1_Transaction
{
    public abstract class AccountBase
    {
        /// <summary>
        /// 帳戶擁有者的名字
        /// </summary>
        public string Name = null;

        /// <summary>
        /// 取得帳戶餘額
        /// </summary>
        /// <returns></returns>
        public abstract decimal GetBalance();

        /// <summary>
        /// 執行交易，將指定金額轉入該帳戶
        /// </summary>
        /// <param name="transferAmount"></param>
        /// <returns></returns>
        public abstract decimal ExecTransaction(decimal transferAmount);
    }

    public class TransactionItem
    {
        public DateTime Date;
        public decimal Amount;
        public string Memo;
    }
}