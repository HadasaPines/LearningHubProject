export type Role = "Student" | "Teacher" | "Admin";
export type Gender = "M" | "F";

export interface User2 {
  userId: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  password: string;
  role: Role;
}

export interface Student {
  studentId: number; // שווה ל-userId
  gender: Gender;
  age: number;
  birthDate: string;
}

export interface UserWithStudent {
  user: User2;
  student: Student;
}
