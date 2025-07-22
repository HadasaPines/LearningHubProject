import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import type { LoginFormData, User } from "../../models/userModel";
import { loginUser } from "../../services/api";
import { parseApiError } from "../../utils/apiErrorParser";
import { FaEye, FaEyeSlash } from "react-icons/fa";

interface Props {
  onToast: (type: "error" | "success", message: string) => void;
}

const LoginForm: React.FC<Props> = ({ onToast }) => {
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);
  const [formData, setFormData] = useState<LoginFormData>({
    userId: 0,
    password: ""
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmitUser = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await loginUser(formData);
      const user: User = response.data;
      localStorage.setItem("user", JSON.stringify(user));
      onToast("success", "Login successful!");
      navigate("/registerLesson");
    } catch (error: any) {
      onToast("error", parseApiError(error));
    }
  };

  return (
    <form onSubmit={handleSubmitUser}>
      <div>
        <label htmlFor="userId">User ID:</label>
        <input
          id="userId"
          name="userId"
          value={formData.userId || ""}
          onChange={handleChange}
        />
      </div>
      <div style={{ position: "relative" }}>
        <label htmlFor="password">Password:</label>
        <input
          type={showPassword ? "text" : "password"}
          id="password"
          name="password"
          value={formData.password}
          onChange={handleChange}
          style={{ paddingRight: "35px" }}
        />
        <span
          onClick={() => setShowPassword((prev) => !prev)}
          style={{
            position: "absolute",
            right: "10px",
            top: "38px",
            transform: "translateY(-50%)",
            cursor: "pointer"
          }}
        >
          {showPassword ? <FaEyeSlash /> : <FaEye />}
        </span>
      </div>
      <button type="submit">Login</button>
    </form>
  );
};

export default LoginForm;
