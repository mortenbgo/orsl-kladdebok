import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService, User } from '../../services/user.service';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: User[] = [];
  loading = false;
  error: string | null = null;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this.error = null;
    
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Kunne ikke laste brukere: ' + error.message;
        this.loading = false;
        console.error('Error loading users:', error);
      }
    });
  }

  addTestUser(): void {
    const newUser = {
      name: 'Test Bruker',
      email: 'test@example.com'
    };

    this.userService.createUser(newUser).subscribe({
      next: (user) => {
        this.users.push(user);
      },
      error: (error) => {
        this.error = 'Kunne ikke opprette bruker: ' + error.message;
        console.error('Error creating user:', error);
      }
    });
  }

  getApiUrl(): string {
    return this.userService.getApiUrl();
  }
}
