export interface Item {
  id: number;
  nome: string;
  quantidade: number;
  precoUnitario: number;
  pedidoId: number;
}

export interface Pedido {
  id: number;
  cliente: string;
  itens: Item[];
  totalPedido: number;
  status: number;
}