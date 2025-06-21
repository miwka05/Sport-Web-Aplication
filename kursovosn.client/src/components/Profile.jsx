import React, { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { authService, logoutUser } from "../services/authService";
import axios from "axios";


const Profile = () => {
    const [user, setUser] = useState(null);
    const [entries, setEntries] = useState({ accepted: [], pending: [], rejected: [] });
    const navigate = useNavigate();
    const [myTournaments, setMyTournaments] = useState([]);
    const [myTeams, setMyTeams] = useState([]);

    useEffect(() => {
        const loadUser = async () => {
            try {
                const data = await authService.getCurrentUser();
                setUser(data);
                fetchEntries();
            } catch (error) {
                console.error("Ошибка при загрузке пользователя:", error);
            }
        };

        const fetchEntries = async () => {
            try {
                const token = sessionStorage.getItem("token");
                const response = await axios.get("http://localhost:5082/api/tournament/my-entries", {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setEntries(response.data);
            } catch (error) {
                console.error("Ошибка при загрузке заявок:", error);
            }
        };

        loadUser();
        fetchMyTournaments();
        fetchMyTeams();
    }, []);

    const fetchMyTournaments = async () => {
        try {
            const token = sessionStorage.getItem("token");
            const response = await axios.get("http://localhost:5082/api/tournament/my", {
                headers: { Authorization: `Bearer ${token}` }
            });
            setMyTournaments(response.data.$values);
        } catch (error) {
            console.error("Ошибка при загрузке ваших турниров:", error);
        }
    };

    const fetchMyTeams = async () => {
        try {
            const token = sessionStorage.getItem("token");
            const response = await axios.get("http://localhost:5082/api/teams/my", {
                headers: { Authorization: `Bearer ${token}` }
            });
            setMyTeams(response.data.$values);
        } catch (error) {
            console.error("Ошибка при загрузке ваших команд:", error);
        }
    };

    if (!user) return <p>Загрузка...</p>;

    return (
        <div style={{ maxWidth: "800px", margin: "auto" }}>
            <h1>Профиль пользователя</h1>
            <ul>
                <li><strong>Имя:</strong> {user.firstName}</li>
                <li><strong>Фамилия:</strong> {user.lastName}</li>
                <li><strong>Email:</strong> {user.email}</li>
                <li><strong>Дата рождения:</strong> {user.dateOfBirth?.slice(0, 10)}</li>
                <li><strong>Пол:</strong> {user.sex}</li>
            </ul>
            <div style={{ marginTop: "20px" }}>
                <button onClick={() => navigate("/editProfile")}>Редактировать профиль</button>
                <button onClick={() => navigate("/changePassword")} style={{ marginLeft: "10px" }}>
                    Сменить пароль
                </button>
                <button onClick={logoutUser}>Выйти</button>
            </div>

            <div style={{ marginTop: "30px" }}>
                <h2>Мои заявки на турниры</h2>

                <h3>Принятые</h3>
                <ul>
                    {(entries.accepted?.$values || []).map((e, i) => (
                        <li key={`accepted-${i}`}>
                            <Link to={`/tournaments/${e.tournamentId}`}>{e.tournamentName}</Link> ({e.teamName ?? e.userName})
                        </li>
                    ))}
                </ul>

                <h3>Ожидают подтверждения</h3>
                <ul>
                    {(entries.pending?.$values || []).map((e, i) => (
                        <li key={`pending-${i}`}>
                            <Link to={`/tournaments/${e.tournamentId}`}>{e.tournamentName}</Link> ({e.teamName ?? e.userName})
                        </li>
                    ))}
                </ul>

                <h3>Отклонённые</h3>
                <ul>
                    {(entries.rejected?.$values || []).map((e, i) => (
                        <li key={`rejected-${i}`}>
                            <Link to={`/tournaments/${e.tournamentId}`}>{e.tournamentName}</Link> ({e.teamName ?? e.userName})
                        </li>
                    ))}
                </ul>

            </div>
            <div style={{ marginTop: "30px" }}>
                <h2>Мои турниры</h2>
                {myTournaments.length === 0 ? (
                    <p>У вас нет созданных турниров.</p>
                ) : (
                    <ul>
                        {myTournaments.map(t => (
                            <li key={t.id}>
                                <Link to={`/tournaments/${t.id}`}>{t.name}</Link>
                            </li>
                        ))}
                    </ul>
                )}
            </div>

            <div style={{ marginTop: "30px" }}>
                <h2>Мои команды</h2>
                {myTeams.length === 0 ? (
                    <p>У вас нет созданных команд.</p>
                ) : (
                    <ul>
                        {myTeams.map(team => (
                            <li key={team.id}>
                                <Link to={`/team/${team.id}`}>{team.name}</Link>
                            </li>
                        ))}
                    </ul>
                )}
            </div>
        </div>
    );
};

export default Profile;

