import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PedidoFormComponent } from './pedidos/pedido-form/pedido-form.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, PedidoFormComponent],
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'angular-pedidos';
}