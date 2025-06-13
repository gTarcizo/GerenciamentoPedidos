import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Pedido, PedidoDto } from '../pedido.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-pedido-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './pedido-form.component.html',
  styleUrls: ['./pedido-form.component.css']
})
export class PedidoFormComponent {
  pedidoForm: FormGroup;

  

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.pedidoForm = this.fb.group({
      cliente: ['', Validators.required],
      itens: this.fb.array([
        this.fb.group({
          nome: ['', Validators.required],
          quantidade: [1, [Validators.required, Validators.min(1)]],
          precoUnitario: [0, [Validators.required, Validators.min(0)]]
        })
      ]),
      totalPedido: [{ value: 0, disabled: true }],
      status: [null, Validators.required]
    });

    this.pedidoForm.get('itens')?.valueChanges.subscribe(() => {
      this.atualizarTotal();
    });
  }

  get itens(): FormArray {
    return this.pedidoForm.get('itens') as FormArray;
  }

  addItem(): void {
    this.itens.push(this.fb.group({
      nome: ['', Validators.required],
      quantidade: [1, [Validators.required, Validators.min(1)]],
      precoUnitario: [0, [Validators.required, Validators.min(0)]]
    }));
  }

  removeItem(index: number): void {
    this.itens.removeAt(index);
    this.atualizarTotal();
  }

  atualizarTotal(): void {
    const total = this.itens.controls.reduce((sum, group) => {
      const quantidade = group.get('quantidade')?.value || 0;
      const preco = group.get('precoUnitario')?.value || 0;
      return sum + quantidade * preco;
    }, 0);
    this.pedidoForm.get('totalPedido')?.setValue(total);
  }

  retornarHeaders(): HttpHeaders {
    const token = (document.getElementById('tokenJWT')?.textContent || '').trim();
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  submit(): void {
  if (this.pedidoForm.invalid) {
    alert('Preencha todos os campos obrigatórios!');
    return;
  }

  const raw = this.pedidoForm.getRawValue();

  const pedidoDto: PedidoDto = {
    id: 0,
    cliente: raw.cliente,
    itens: raw.itens.map((item: any) => ({
      id: 0,
      nome: item.nome,
      quantidade: Number(item.quantidade),
      precoUnitario: Number(item.precoUnitario),
      pedidoId: 0
    })),
    totalPedido: raw.totalPedido,
    status: Number(raw.status)
  };

  const headers = this.retornarHeaders();

  this.http.post('http://localhost:5109/pedido', pedidoDto, { headers }).subscribe({
    next: () => alert('Pedido enviado com sucesso!'),
    error: (err) => {
      if (err.status === 401) {
        alert('Não autorizado');
      } else {
        alert('Erro ao enviar pedido');
        console.error(err);
      }
    }
  });
}
}