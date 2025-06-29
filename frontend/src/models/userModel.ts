export type Role = "Teacher" | "Student" | "Admin";
export type Gender = "Male" | "Female";

export interface StudentDetails {
  gender: Gender;
  age: number;
  birthDate: string;
}

export interface RegisterFormData {
  userId: number;
  firstName: string;
  lastName: string;
  password: string;
  phone: string;
  email: string;
  role: "Student";
  studentDetails: StudentDetails;
}

export interface LoginFormData {
  userId: number;
  password: string;
  // email: string;
}
