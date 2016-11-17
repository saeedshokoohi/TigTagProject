using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO;
using TigTag.DTO.ModelDTO.Base;
using TigTag.Repository.IModelRepository;
using TiTag.Repository.Base;


namespace TigTag.Repository.ModelRepository {

    public class InvoiceTransactionRepository :
        GenericRepository<DataModelContext, InvoiceTransaction>, IInvoiceTransactionRepository
    {
        PageRepository pageRepo = new PageRepository();

        public InvoiceTransaction GetSingle(Guid Id)
        {
            var query = Context.InvoiceTransactions.FirstOrDefault(x => x.Id == Id);
            return query;
        }

        public void addNewPaymentTransaction(Invoice invoice, PaymentInfo payInfo)
        {
            InvoiceTransaction newTransaction = new InvoiceTransaction();
            newTransaction.Id = Guid.NewGuid();
            newTransaction.CreateDate = DateTime.Now;
            newTransaction.Invoice = invoice;
            newTransaction.StatusCode = payInfo.status;
            newTransaction.Comment = payInfo.payGateUrl;
            newTransaction.Authority = payInfo.payAuthority;
       
            
            Context.InvoiceTransactions.Add(newTransaction);
            Context.SaveChanges();
        }
    }
       
}