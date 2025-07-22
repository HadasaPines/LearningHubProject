import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import type { User, StudentDetails } from "../models/userModel";
import { addUser, addStudent } from "../services/api";
import { parseApiError } from "../utils/apiErrorParser";
import { FaEye, FaEyeSlash } from "react-icons/fa";
import styles from "./authPage.module.scss"

interface Props {
  onToast: (type: "error" | "success", message: string) => void;
}

const RegisterForm: React.FC<Props> = ({ onToast }) => {
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);
  const [birthDateType, setBirthDateType] = useState<"text" | "date">("text");

  const [studentData, setStudentData] = useState<Omit<StudentDetails, "studentId">>({
    age: 0,
    birthDate: "",
    gender: "",
  });

  const [UserData, setUserData] = useState<User>({
    userId: 0,
    firstName: "",
    lastName: "",
    password: "",
    phone: "",
    email: "",
    role: "Student",
    student: studentData,
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setUserData((prev) => ({
      ...prev,
      [name]: name === "userId" ? Number(value) : value,
    }));
  };

  const handleNestedChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    let newStudent = { ...studentData, [name]: value };

    if (name === "birthDate") {
      const birthDate = new Date(value);
      const today = new Date();
      let age = today.getFullYear() - birthDate.getFullYear();
      const m = today.getMonth() - birthDate.getMonth();
      if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
      }
      newStudent.age = age;
    }

    setStudentData(newStudent);
  };

  const handleSubmitUser = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addUser(UserData);
      await addStudent(studentData);
      onToast("success", "Registration successful!");
      navigate("/registerLesson");
    } catch (error: any) {
      onToast("error", parseApiError(error));
    }
  };

  return (
    <form onSubmit={handleSubmitUser}>
      <input name="userId" placeholder="ID" value={UserData.userId || ""} onChange={handleChange} required />
      <input name="firstName" placeholder="First Name" value={UserData.firstName} onChange={handleChange} required />
      <input name="lastName" placeholder="Last Name" value={UserData.lastName} onChange={handleChange} required />
      <input name="phone" placeholder="Phone" value={UserData.phone} onChange={handleChange} required />
      <input type="email" name="email" placeholder="Email" value={UserData.email} onChange={handleChange} required />

      <div className={styles["password-input-wrapper"]}>
        <input
          type={showPassword ? "text" : "password"}
          name="password"
          placeholder="Password"
          value={UserData.password}
          onChange={handleChange}
        />
        <span className={styles["eye-icon"]} onClick={() => setShowPassword((prev) => !prev)}>
          {showPassword ? <FaEyeSlash /> : <FaEye />}
        </span>
      </div>

      <select name="gender" value={studentData.gender || ""} onChange={handleNestedChange} required>
        <option value="">Select Gender</option>
        <option value="M">Male</option>
        <option value="F">Female</option>
      </select>

      <input
        type={birthDateType}
        name="birthDate"
        value={studentData.birthDate}
        onChange={handleNestedChange}
        onFocus={() => setBirthDateType("date")}
        onBlur={() => {
          if (!studentData.birthDate) setBirthDateType("text");
        }}
        placeholder="Birth Date"
        required
      />

      <input type="text" name="age" placeholder="Age" value={studentData.age || ""} disabled />

      <button type="submit">Sign Up</button>
    </form>
  );
};

export default RegisterForm;
