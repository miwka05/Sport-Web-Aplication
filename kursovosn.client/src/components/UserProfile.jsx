import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";

const UserProfile = () => {
    const { userId } = useParams();  // Получаем userId из параметров URL
    const [user, setUser] = useState(null);
    const [entries, setEntries] = useState({ accepted: [], pending: [], rejected: [] });
    const [banReason, setBanReason] = useState('');
    const [isAdmin, setIsAdmin] = useState(false);

    useEffect(() => {
        // Функция для загрузки данных пользователя
        const loadUserProfile = async () => {
            try {
                const response = await axios.get(`http://localhost:5082/api/auth/user/${userId}`);
                setUser(response.data);
                fetchEntries();
            } catch (error) {
                console.error("Ошибка при загрузке чужого профиля:", error);
            }
        };

        const fetchEntries = async () => {
            try {
                const token = sessionStorage.getItem("token");
                const response = await axios.get(`http://localhost:5082/api/tournament/user-entries/${userId}`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setEntries(response.data);
            } catch (error) {
                console.error("Ошибка при загрузке заявок:", error);
            }
        };
        const token = sessionStorage.getItem("token");
        if (token) {
            const payload = JSON.parse(atob(token.split('.')[1]));
            if (payload && payload["sub"] === "admin") {
                setIsAdmin(true);
            }
        }


        loadUserProfile();
    }, [userId]);

    const handleBanUser = async () => {
        if (!banReason.trim()) {
            alert("Введите причину бана");
            return;
        }
        try {
            const token = sessionStorage.getItem("token");
            await axios.post(`http://localhost:5082/api/auth/${userId}/ban`,
                { reasonBan: banReason },
                { headers: { Authorization: `Bearer ${token}` } }
            );
            alert("Пользователь заблокирован");
        } catch (error) {
            console.error("Ошибка при блокировке пользователя:", error);
            alert("Ошибка при блокировке пользователя");
        }
    };

    if (!user) return <p>Загрузка...</p>;

    if (user?.ban === true && !isAdmin ) {
        return (
            <div style={{ padding: "2rem", color: "red" }}>
                <h2>Этот пользователь был заблокирован администрацией</h2>
                <p>Причина: {user.reasonBan || 'не указана'}</p>
            </div>
        );
    }

    return (
        <div style={{ maxWidth: "800px", margin: "auto" }}>
            <h1>Профиль пользователя {user.firstName} {user.lastName}</h1>
            <ul>
                <li><strong>Имя:</strong> {user.firstName}</li>
                <li><strong>Фамилия:</strong> {user.lastName}</li>
                <li><strong>Email:</strong> {user.email}</li>
                <li><strong>Дата рождения:</strong> {user.dateOfBirth?.slice(0, 10)}</li>
                <li><strong>Пол:</strong> {user.sex}</li>
            </ul>
            {isAdmin && (
                <div style={{ marginTop: "20px", borderTop: "1px solid gray", paddingTop: "10px" }}>
                    <h3>Блокировка пользователя</h3>
                    <textarea
                        placeholder="Причина блокировки"
                        value={banReason}
                        onChange={(e) => setBanReason(e.target.value)}
                        rows={3}
                        style={{ width: "100%", marginBottom: "10px" }}
                    />
                    <button onClick={handleBanUser}>Заблокировать пользователя</button>
                </div>
            )}
            <div style={{ marginTop: "20px" }}>
                <h2>Заявки на турниры</h2>

                <h3>Принятые</h3>
                <ul>
                    {(entries.accepted?.$values || []).map((e, i) => (
                        <li key={`accepted-${i}`}>{e.tournamentName} ({e.teamName ?? e.userName})</li>
                    ))}
                </ul>

                <h3>Ожидают подтверждения</h3>
                <ul>
                    {(entries.pending?.$values || []).map((e, i) => (
                        <li key={`pending-${i}`}>{e.tournamentName} ({e.teamName ?? e.userName})</li>
                    ))}
                </ul>

                <h3>Отклонённые</h3>
                <ul>
                    {(entries.rejected?.$values || []).map((e, i) => (
                        <li key={`rejected-${i}`}>{e.tournamentName} ({e.teamName ?? e.userName})</li>
                    ))}
                </ul>
            </div>
        </div>
    );
};

export default UserProfile;
