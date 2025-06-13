import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PedidoFormComponent } from './pedidos/pedido-form/pedido-form.component';
import { AutenticacaoComponent } from './autenticacao/autenticacao.component';
import { PedidoListComponent } from './pedidos/pedido-list/pedido-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, PedidoFormComponent, AutenticacaoComponent, PedidoListComponent],
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'Gerenciador de Pedidos';
}