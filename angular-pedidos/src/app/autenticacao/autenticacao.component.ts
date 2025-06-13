import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-autenticacao',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './autenticacao.component.html',
  styleUrls: ['./autenticacao.component.css']

})
export class AutenticacaoComponent {
  token: string | null = null;

  constructor(private http: HttpClient) {}

  autenticar() {
    this.http.post<{ token: string }>('http://localhost:5109/token', {})
      .subscribe({
        next: (response) => {
          this.token = response.token;
          console.log('Token recebido:', this.token);
        },
        error: (error) => {
          console.error('Erro ao autenticar:', error);
        }
      });
  }
}
