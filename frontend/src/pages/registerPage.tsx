import React, { useState } from "react";
import type { RegisterFormData, Gender } from "../models/userModel";
import { addUser, addStudent } from "../services/api";
import { parseApiError } from "../utils/apiErrorParser";
import { useNavigate } from "react-router-dom";

const RegisterForm: React.FC = () => {
  const [errorMessages, setErrorMessages] = useState<string | null>(null);
   const navigate = useNavigate();
  const [formData, setFormData] = useState<RegisterFormData>({
    userId: 0,
    firstName: "",
    lastName: "",
    password: "",
    phone: "",
    email: "",
    role: "Student",
      studentDetails: {
    gender: "Male", 
    age: 0,
    birthDate: ""
  }
  });


  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === "userId" ? Number(value) : value,
    }));
  };

  const handleNestedChange = (
     e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>,
     section: "studentDetails"
   ) => {
     const { name, value } = e.target;
     setFormData((prev) => ({
       ...prev,
       [section]: {
         ...prev[section],
         [name]: value,
       },
     }));
   };

  const handleSubmitUser = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addUser(formData);
    await addStudent({
          studentId: formData.userId,
          gender: convertGender(formData.studentDetails.gender),
          age: formData.studentDetails.age,
          birthDate: formData.studentDetails.birthDate,
        });
         navigate("/");
      setErrorMessages(null);
    } catch (error: any) {
      setErrorMessages(parseApiError(error));
    }
  };

  const convertGender = (gender: Gender): "M" | "F" =>
    gender === "Male" ? "M" : "F";

  
  return (
    <>
      {errorMessages && (
        <div role="alert" aria-live="assertive">
          {errorMessages}
        </div>
      )}

      <form onSubmit={ handleSubmitUser}>
        <h2>Signup</h2>
          <>
            <input
              name="userId"
              placeholder="ID Number"
              value={formData.userId}
              onChange={handleChange}
            />
            <input
              name="firstName"
              placeholder="First Name"
              value={formData.firstName}
              onChange={handleChange}
            />
            <input
              name="lastName"
              placeholder="Last Name"
              value={formData.lastName}
              onChange={handleChange}
            />
            <input
              name="phone"
              placeholder="Phone"
              value={formData.phone}
              onChange={handleChange}
            />
            <input
              type="email"
              name="email"
              placeholder="Email"
              value={formData.email}
              onChange={handleChange}
            />
            <input
              type="password"
              name="password"
              placeholder="Password"
              value={formData.password}
              onChange={handleChange}
            />
            <select name="role" value={formData.role} disabled>
              <option value="Student">Student</option>
            </select>
          </>
          <>
            <select
              name="gender"
              onChange={(e) => handleNestedChange(e, "studentDetails")}
              required
            >
              <option value="">Select Gender</option>
              <option value="Male">Male</option>
              <option value="Female">Female</option>
            </select>
            <input
              type="number"
              name="age"
              placeholder="Age"
              onChange={(e) => handleNestedChange(e, "studentDetails")}
              required
            />
            <input
              type="date"
              name="birthDate"
              onChange={(e) => handleNestedChange(e, "studentDetails")}
              required
            />
          </>

        <button type="submit">Finish</button>
      </form>
    </>
  );
};

export default RegisterForm;
