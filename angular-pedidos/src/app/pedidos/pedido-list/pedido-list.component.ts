import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { Pedido } from '../pedido.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-pedido-list',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FormsModule],
  templateUrl: './pedido-list.component.html',
  styleUrls: ['./pedido-list.component.css']

})
export class PedidoListComponent {
  pedidos: any[] = [];

  constructor(private http: HttpClient) {}

  carregarPedidos() {
    var headers:HttpHeaders = this.retornarHeaders();
    this.http.get<any[]>('http://localhost:5109/pedidos', { headers }).subscribe({
      next: (data) => {
        this.pedidos = data;
        console.log('Status de cada pedido:', data.map(p => typeof p.status));
      },
      error: (err) => {
        if (err.status === 401) {
          alert('Alerta! Você não tem autorização para carregar a lista de pedidos fazer isso. Clique em autenticar para receber acesso');
        } else {
          console.error('Erro ao buscar pedidos:', err);
        }
      }
    });
  }
  atualizarStatus(pedido: Pedido) {
    const url = `http://localhost:5109/pedido/${pedido.id}?status=${pedido.status}`;
    var headers:HttpHeaders = this.retornarHeaders();
    this.http.put(url, null, { headers }).subscribe({
      next: () => alert('Status atualizado com sucesso!'),
      error: () => alert('Erro ao atualizar status')
    });
  }
  
  retornarHeaders(): HttpHeaders {
  const token = (document.getElementById('tokenJWT')?.textContent || '').trim();
  return new HttpHeaders().set('Authorization', `Bearer ${token}`);
}
}
