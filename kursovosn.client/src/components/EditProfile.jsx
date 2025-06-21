import { useEffect, useState } from "react";
import { authService } from "../services/authService";
import { userService } from "../services/userService";

const EditProfile = () => {
    const [form, setForm] = useState({
        firstName: '',
        secondName: '',
        email: '',
        birthDate: '',
        pol: ''
    });


    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchProfile = async () => {
            try {
                const user = await authService.getCurrentUser();
                setForm({
                    firstName: user.firstName || "",
                    secondName: user.lastName || "",
                    email: user.email || "",
                    birthDate: user.dateOfBirth?.slice(0, 10) || "", // YYYY-MM-DD
                    pol: user.sex || ""
                });
                setLoading(false);
            } catch (error) {
                console.error("Ошибка при загрузке профиля:", error);
            }
        };

        fetchProfile();
    }, []);

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await userService.updateProfile(form);
            alert("Профиль обновлён");
        } catch (error) {
            console.error("Ошибка при обновлении профиля:", error);
        }
    };

    if (loading) return <p>Загрузка...</p>;

    return (
        <form onSubmit={handleSubmit}>
            <label>Имя:</label>
            <input
                type="text"
                name="firstName"
                value={form.firstName}
                onChange={handleChange}
            />

            <label>Фамилия:</label>
            <input
                type="text"
                name="secondName"
                value={form.secondName}
                onChange={handleChange}
            />

            <label>Email:</label>
            <input
                type="email"
                name="email"
                value={form.email}
                onChange={handleChange}
            />

            <label>Дата рождения:</label>
            <input
                type="date"
                name="birthDate"
                value={form.birthDate}
                onChange={handleChange}
            />

            <label>Пол:</label>
            <select name="pol" value={form.pol} onChange={handleChange}>
                <option value="">Выберите</option>
                <option value="Male">Мужской</option>
                <option value="Female">Женский</option>
            </select>

            <button type="submit">Сохранить</button>
        </form>
    );
};

export default EditProfile;
