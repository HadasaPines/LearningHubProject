import React, { useState } from "react";
import type {User } from "../models/userModel";
import type { StudentDetails } from "../models/userModel";
import { addUser, addStudent } from "../services/api";
import { parseApiError } from "../utils/apiErrorParser";
import { useNavigate } from "react-router-dom";


const RegisterForm: React.FC = () => {
  const [errorMessages, setErrorMessages] = useState<string | null>(null);
   const navigate = useNavigate();
   const[studentData, setStudentData] = useState<Omit<StudentDetails, "studentId">>({
    age: 0,
    birthDate: "",
    gender: "M"
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


  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
  
    setUserData((prev) => ({
      ...prev,
      [name]: name === "userId" ? Number(value) : value,
    }));
  };

  const handleNestedChange = (
     e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>,
   ) => {
     const { name, value } = e.target;
if(name=="birthDate"){
     const birthDate = new Date(value);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    studentData.age = age;}
     setStudentData((prev) => ({
       ...prev,
     [name]:value
     }));
   };

  const handleSubmitUser = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await addUser(UserData);
    await addStudent(studentData);
         navigate("/");
      setErrorMessages(null);
    } catch (error: any) {
      setErrorMessages(parseApiError(error));
    }
  };
  
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
              value={UserData.userId}
              onChange={handleChange}
            />
            <input
              name="firstName"
              placeholder="First Name"
              value={UserData.firstName}
              onChange={handleChange}
            />
            <input
              name="lastName"
              placeholder="Last Name"
              value={UserData.lastName}
              onChange={handleChange}
            />
            <input
              name="phone"
              placeholder="Phone"
              value={UserData.phone}
              onChange={handleChange}
            />
            <input
              type="email"
              name="email"
              placeholder="Email"
              value={UserData.email}
              onChange={handleChange}
            />
            <input
              type="password"
              name="password"
              placeholder="Password"
              value={UserData.password}
              onChange={handleChange}
            />
          
          </>
          <>
            <select
              name="gender"
              value={studentData.gender}
              onChange={(e) => handleNestedChange(e)}
              required
             
            >
              <option value="">Select Gender</option>
              <option value="M">Male</option>
              <option value="F">Female</option>
            </select>
            <input
              type="text"
              name="age"
              placeholder="Age"
              value={studentData.age}
              onChange={(e) => handleNestedChange(e)}
              required
            />
            <input
              type="date"
              name="birthDate"
              value={studentData.birthDate}
              onChange={(e) => handleNestedChange(e)}
              required
            />
          </>

        <button type="submit">Finish</button>
      </form>
    </>
  );
};

export default RegisterForm;
