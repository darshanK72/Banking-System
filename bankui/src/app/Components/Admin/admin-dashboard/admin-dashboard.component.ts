import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../Services/admin.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css'],
})
export class AdminDashboardComponent implements OnInit {
  users: any[] = [];
  selectedUser: any = null;
  isEditing: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private adminService: AdminService
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.adminService.getUsers().subscribe(
      (users) => {
        this.users = users;
      },
      (error) => {
        console.error('Failed to load users', error);
      }
    );
  }

  viewUserDetails(user: any): void {
    this.selectedUser = user;
    this.isEditing = false;
  }

  editUser(user: any): void {
    this.selectedUser = { ...user };
    this.isEditing = true;
  }

  saveUser(): void {
    this.adminService.updateUser(this.selectedUser).subscribe(
      () => {
        this.loadUsers();
        this.selectedUser = null;
        this.isEditing = false;
      },
      (error) => {
        console.error('Failed to update user', error);
      }
    );
  }

  approveUser(user: any): void {
    this.adminService.approveUser(user.id).subscribe(
      () => {
        this.loadUsers();
      },
      (error) => {
        console.error('Failed to approve user', error);
      }
    );
  }

  cancelEdit(): void {
    this.selectedUser = null;
    this.isEditing = false;
  }

  deleteUser(userId: number): void {
    this.adminService.deleteUser(userId).subscribe(() => {
      this.loadUsers();
    });
  }
}
