using MiniETrade.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Application.Common.Abstractions.Persistence.Repositories.Orders;

public interface IOrderWriteRepository : IWriteRepository<Order>
{}