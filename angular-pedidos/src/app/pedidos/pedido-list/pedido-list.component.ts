import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-pedido-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './pedido-list.component.html',
})
export class PedidoListComponent {
  todosPedidos: any[] = [];
  pedidosFiltrados: any[] = [];
  filtroStatus: number | null = null;

  constructor(private http: HttpClient) {
  }

  retornarHeaders(): HttpHeaders {
    const token = (document.getElementById('tokenJWT')?.textContent || '').trim();
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  carregarPedidos(): void {
    const headers = this.retornarHeaders();
    this.http.get<any[]>('http://localhost:5109/pedidos', { headers }).subscribe({
      next: data => {
        this.todosPedidos = data;
        this.aplicarFiltro(); 
      },
      error: err => {
        if (err.status === 401) {
          alert('Não autorizado');
        } else {
          alert('Erro ao carregar pedidos');
        }
      }
    });
  }

  aplicarFiltro(): void {
    if (this.filtroStatus === null) {
      this.pedidosFiltrados = [...this.todosPedidos];
    } else {
      this.pedidosFiltrados = this.todosPedidos.filter(p => p.status === this.filtroStatus);
    }
  }

  atualizarStatus(pedido: any): void {
    const headers = this.retornarHeaders();
    const url = `http://localhost:5109/pedido/${pedido.id}?status=${pedido.status}`;

    this.http.put(url, null, { headers }).subscribe({
      next: () => alert('Status atualizado com sucesso!'),
      error: err => {
        if (err.status === 401) {
          alert('Não autorizado');
        } else {
          alert('Erro ao atualizar status');
        }
      }
    });
  }
}