import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Pedido, Item } from '../pedido.model';

@Component({
  selector: 'app-pedido-form',
  templateUrl: './pedido-form.component.html',
  styleUrls: ['./pedido-form.component.css']
})
export class PedidoFormComponent implements OnInit {
  pedidoForm!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.pedidoForm = this.fb.group({
      cliente: ['', Validators.required],
      itens: this.fb.array([]),
      status: [1, Validators.required] // Pode ser alterado conforme seu enum
    });

    this.addItem(); // inicia com 1 item
  }

  get itens() {
    return this.pedidoForm.get('itens') as FormArray;
  }

  addItem() {
    const itemGroup = this.fb.group({
      id: [0],
      nome: ['', Validators.required],
      quantidade: [0, [Validators.required, Validators.min(1)]],
      precoUnitario: [0, [Validators.required, Validators.min(0.01)]],
      pedidoId: [0]
    });

    this.itens.push(itemGroup);
  }

  removeItem(index: number) {
    this.itens.removeAt(index);
  }

  calcularTotal(): number {
    return this.itens.controls.reduce((total, item) => {
      const quantidade = item.get('quantidade')?.value || 0;
      const precoUnitario = item.get('precoUnitario')?.value || 0;
      return total + (quantidade * precoUnitario);
    }, 0);
  }

  onSubmit() {
    if (this.pedidoForm.valid) {
      const pedido: Pedido = {
        id: 0, // novo pedido
        cliente: this.pedidoForm.value.cliente,
        itens: this.pedidoForm.value.itens,
        totalPedido: this.calcularTotal(),
        status: this.pedidoForm.value.status
      };

      console.log('Pedido a enviar:', pedido);
      // Aqui você chama o service para enviar à API
    } else {
      this.pedidoForm.markAllAsTouched();
    }
  }
}