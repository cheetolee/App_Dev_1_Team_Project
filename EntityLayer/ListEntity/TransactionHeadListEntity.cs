using System;

namespace EntityLayer
{
    public class TransactionHeadListEntity : IComparable<TransactionHeadListEntity>
    {
        public TransactionHeadListEntity() {
            Head = new TransactionHeadEntity();
        }
        public TransactionHeadListEntity(TransactionHeadEntity head, UserEntity user)
            :this()
        {
            Head = head;
            User = user;
        }
        public TransactionHeadListEntity(TransactionHeadEntity head, UserEntity user, decimal listVariable)
            : this(head,user)
        {
            ListVariable = listVariable;
        }

        public TransactionHeadEntity Head { get; set; }
        public UserEntity User { get; set; }

        public decimal ListVariable { get; set; }

        public int CompareTo(TransactionHeadListEntity other)
        {
            if (Head == null)
                return User.CompareTo(other.User);
            return Head.Date.CompareTo(other.Head.Date);
        }
    }
}