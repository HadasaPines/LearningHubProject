import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import type { LoginFormData } from "../models/userModel";
import { loginUser } from "../services/api";
import { parseApiError } from "../utils/apiErrorParser";

const LoginForm: React.FC = () => {
const [errorMessages, setErrorMessages] = useState<string | null>(null);
 const navigate = useNavigate();
  const [formData, setFormData] = useState<LoginFormData>({
    userId: 0,
    password: ""
    });
  
  

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };



  const handleSubmitUser = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await loginUser(formData);
      navigate("/home");
      setErrorMessages(null);
    } catch (error: any) {
      setErrorMessages(parseApiError(error));
    }
  };
;



  return (
    <>
      {errorMessages && (
        <div role="alert" aria-live="assertive">
          {errorMessages}
        </div>
      )}
 
 <form onSubmit={handleSubmitUser}>
        <div>
          <label htmlFor="userId">User ID:</label>
          <input

            id="userId"
            name="userId"
            value={formData.userId}
            onChange={handleChange}

          />
        </div>
        <div>
          <label htmlFor="password">Password:</label>
          <input
            type="password"
            id="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
 
          />
        </div>
        <button type="submit">Login</button>
      </form>
     
    </>
  );
};

export default LoginForm;
