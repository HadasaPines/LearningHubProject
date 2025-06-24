import React, { useState } from "react";
import type { SignupFormData, Gender } from "../models/userModel";
import { addUser, addTeacher, addStudent } from "../services/api";
import { parseApiError } from "../utils/apiErrorParser";  // <-- הוספה כאן

const SignupForm: React.FC = () => {
  const [step, setStep] = useState<1 | 2>(1);
  const [formData, setFormData] = useState<SignupFormData>({
    userId: 0,
    firstName: "",
    lastName: "",
    password: "",
    phone: "",
    email: "",
    role: "Student",
  });
  const [errorMessages, setErrorMessages] = useState<string | null>(null); // <-- הוספה כאן

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
    section: "teacherDetails" | "studentDetails"
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
      setStep(2);
      setErrorMessages(null); // ניקוי שגיאות קודמות
    } catch (error: any) {
      setErrorMessages(parseApiError(error));
    }
  };

  const convertGender = (gender: Gender): "M" | "F" =>
    gender === "Male" ? "M" : "F";

  const handleSubmitDetails = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      if (formData.role === "Teacher" && formData.teacherDetails) {
        await addTeacher({
          teacherId: formData.userId,
          bio: formData.teacherDetails.bio || "",
          gender: convertGender(formData.teacherDetails.gender),
          birthDate: formData.teacherDetails.birthDate,
        });
      } else if (formData.role === "Student" && formData.studentDetails) {
        await addStudent({
          studentId: formData.userId,
          gender: convertGender(formData.studentDetails.gender),
          age: formData.studentDetails.age,
          birthDate: formData.studentDetails.birthDate,
        });
      }
      alert("Details submitted successfully!");
      setErrorMessages(null); // ניקוי שגיאות קודמות
    } catch (error: any) {
      setErrorMessages(parseApiError(error));
    }
  };

  return (
    <>
      {errorMessages && (
        <div
          role="alert"
          aria-live="assertive"
        >
          {errorMessages}
        </div>
      )}

      <form onSubmit={step === 1 ? handleSubmitUser : handleSubmitDetails}>
        <h2>Signup</h2>

        {step === 1 && (
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
            <select name="role" value={formData.role} onChange={handleChange} required>
              <option value="Student">Student</option>
              <option value="Teacher">Teacher</option>
              <option value="Admin">Admin</option>
            </select>
          </>
        )}

        {step === 2 && formData.role === "Teacher" && (
          <>
            <select
              name="gender"
              onChange={(e) => handleNestedChange(e, "teacherDetails")}
              required
            >
              <option value="">Select Gender</option>
              <option value="Male">Male</option>
              <option value="Female">Female</option>
            </select>
            <input
              name="bio"
              placeholder="Biography (optional)"
              onChange={(e) => handleNestedChange(e, "teacherDetails")}
            />
            <input
              type="date"
              name="birthDate"
              onChange={(e) => handleNestedChange(e, "teacherDetails")}
              required
            />
          </>
        )}

        {step === 2 && formData.role === "Student" && (
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
        )}

        <button type="submit">{step === 1 ? "Continue" : "Finish"}</button>
      </form>
    </>
  );
};

export default SignupForm;
