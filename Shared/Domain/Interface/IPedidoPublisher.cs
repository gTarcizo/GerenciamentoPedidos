﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Domain.Interface
{
   public interface IPedidoPublisher
   {
      Task PublicarPedidoNoRabbitMQ(Pedido pedido);
   }
}
