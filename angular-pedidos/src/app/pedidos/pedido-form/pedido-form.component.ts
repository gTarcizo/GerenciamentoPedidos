import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Pedido } from '../pedido.model';

@Component({
  selector: 'app-pedido-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './pedido-form.component.html'
})
export class PedidoFormComponent implements OnInit {
  pedidoForm!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.pedidoForm = this.fb.group({
      id: [0],
      cliente: ['', Validators.required],
      status: [1],
      totalPedido: [{ value: 0, disabled: true }],
      itens: this.fb.array([this.criarItem()])
    });

    this.itens.valueChanges.subscribe(() => {
      this.atualizarTotal();
    });
  }

  get itens(): FormArray {
    return this.pedidoForm.get('itens') as FormArray;
  }

  criarItem(): FormGroup {
    return this.fb.group({
      id: [0],
      nome: ['', Validators.required],
      quantidade: [1, [Validators.required, Validators.min(1)]],
      precoUnitario: [0, [Validators.required, Validators.min(0)]],
      pedidoId: [0]
    });
  }

  addItem(): void {
    this.itens.push(this.criarItem());
  }

  removeItem(index: number): void {
    if (this.itens.length > 1) {
      this.itens.removeAt(index);
      this.atualizarTotal();
    }
  }

  atualizarTotal(): void {
    const total = this.itens.controls.reduce((sum, item) => {
      const qtd = item.get('quantidade')?.value || 0;
      const preco = item.get('precoUnitario')?.value || 0;
      return sum + qtd * preco;
    }, 0);

    this.pedidoForm.get('totalPedido')?.setValue(total, { emitEvent: false });
  }

  submitModel(): void {
    if (this.pedidoForm.valid) {
      const pedido: Pedido = this.pedidoForm.getRawValue();
      console.log('Pedido enviado:', pedido);
    } else {
      console.log('Formulário inválido');
    }
  }
}