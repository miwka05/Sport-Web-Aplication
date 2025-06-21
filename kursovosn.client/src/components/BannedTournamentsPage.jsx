import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';

const BannedTournamentsPage = () => {
    const [bannedTournaments, setBannedTournaments] = useState([]);
    const [bannedTeams, setBannedTeams] = useState([]);
    const [bannedUsers, setBannedUsers] = useState([]);
    const [isAdmin, setIsAdmin] = useState(false);

    useEffect(() => {
        const token = sessionStorage.getItem("token");
        if (!token) return;

        if (token) {
            const payload = JSON.parse(atob(token.split('.')[1]));
            if (payload && payload["sub"] === "admin") {
                setIsAdmin(true);
                fetchBannedTournaments(token);
                fetchBannedTeams(token);
                fetchBannedUsers(token);
            }
        }
    }, []);

    const fetchBannedTournaments = async (token) => {
        try {
            const response = await axios.get("http://localhost:5082/api/tournament/banned", {
                headers: { Authorization: `Bearer ${token}` }
            });
            setBannedTournaments(response.data.$values);
        } catch (error) {
            console.error("Ошибка загрузки забаненных турниров:", error);
        }
    };

    const fetchBannedTeams = async (token) => {
        try {
            const response = await axios.get("http://localhost:5082/api/teams/banned", {
                headers: { Authorization: `Bearer ${token}` }
            });
            setBannedTeams(response.data.$values);
        } catch (error) {
            console.error("Ошибка загрузки забаненных команд:", error);
        }
    };

    const fetchBannedUsers = async (token) => {
        try {
            const response = await axios.get("http://localhost:5082/api/auth/banned", {
                headers: { Authorization: `Bearer ${token}` }
            });
            setBannedUsers(response.data.$values);
        } catch (error) {
            console.error("Ошибка загрузки забаненных пользователей:", error);
        }
    };

    const handleUnbanTournament = async (tournamentId) => {
        const token = sessionStorage.getItem("token");
        try {
            await axios.post(`http://localhost:5082/api/tournament/${tournamentId}/unban`, {}, {
                headers: { Authorization: `Bearer ${token}` }
            });
            alert("Турнир разблокирован");
            fetchBannedTournaments(token);
        } catch (error) {
            console.error("Ошибка при разбане турнира:", error);
        }
    };

    const handleUnbanTeam = async (teamId) => {
        const token = sessionStorage.getItem("token");
        try {
            await axios.post(`http://localhost:5082/api/teams/${teamId}/unban`, {}, {
                headers: { Authorization: `Bearer ${token}` }
            });
            alert("Команда разблокирован");
            fetchBannedTeams(token);
        } catch (error) {
            console.error("Ошибка при разбане команды:", error);
        }
    };

    const handleUnbanUser = async (teamId) => {
        const token = sessionStorage.getItem("token");
        try {
            await axios.post(`http://localhost:5082/api/auth/${teamId}/unban`, {}, {
                headers: { Authorization: `Bearer ${token}` }
            });
            alert("Пользователь разблокирован");
            fetchBannedUsers(token);
        } catch (error) {
            console.error("Ошибка при разбане пользователя:", error);
        }
    };

    if (!isAdmin) return <p>Доступ запрещен</p>;

    return (
        <div style={{ maxWidth: '800px', margin: 'auto' }}>
            <h2>Заблокированные турниры</h2>
            {bannedTournaments.length === 0 ? (
                <p>Нет заблокированных турниров</p>
            ) : (
                <ul>
                    {bannedTournaments.map(t => (
                        <li key={t.id} style={{ marginBottom: '10px' }}>
                            <strong>
                                <Link to={`/tournaments/${t.id}`}>{t.name}</Link>
                            </strong><br />
                            Причина: {t.reason}<br />
                            <button onClick={() => handleUnbanTournament(t.id)}>Разблокировать</button>
                        </li>
                    ))}
                </ul>
            )}
            <h2>Заблокированные команды</h2>
            {bannedTeams.length === 0 ? (
                <p>Нет заблокированных команд</p>
            ) : (
                <ul>
                    {bannedTeams.map(t => (
                        <li key={t.id} style={{ marginBottom: '10px' }}>
                            <strong>
                                <Link to={`/team/${t.id}`}>{t.name}</Link>
                            </strong><br />
                            Причина: {t.reason}<br />
                            <button onClick={() => handleUnbanTeam(t.id)}>Разблокировать</button>
                        </li>
                    ))}
                </ul>
            )}
            <h2>Заблокированные пользователи</h2>
            {bannedUsers.length === 0 ? (
                <p>Нет заблокированных пользователей</p>
            ) : (
                <ul>
                    {bannedUsers.map(t => (
                        <li key={t.id} style={{ marginBottom: '10px' }}>
                            <strong>
                                <Link to={`/profile/${t.id}`}>{t.userName}</Link>
                            </strong><br />
                            Причина: {t.reason}<br />
                            <button onClick={() => handleUnbanUser(t.id)}>Разблокировать</button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
};

export default BannedTournamentsPage;