import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import axios from 'axios';

const TeamDetails = () => {
    const { id } = useParams();
    const [team, setTeam] = useState(null);
    const [players, setPlayers] = useState([]);
    const [hasRequested, setHasRequested] = useState(false);
    const [banReason, setBanReason] = useState('');
    const [isAdmin, setIsAdmin] = useState(false);
    const [currentUserId, setCurrentUserId] = useState(null);


    useEffect(() => {
        axios.get(`http://localhost:5082/api/teams/${id}`)
            .then(res => setTeam(res.data))
            .catch(err => console.error(err));

        checkJoinRequest();
        axios.get(`http://localhost:5082/api/teams/${id}/players`)
            .then(res => setPlayers(res.data.values.$values || []))
            .catch(err => console.error(err));

        const token = sessionStorage.getItem("token");
        if (token) {
            const payload = JSON.parse(atob(token.split('.')[1]));
            setCurrentUserId(payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]);
            if (payload && payload["sub"] === "admin") {
                setIsAdmin(true);
            }
        }
    }, [id]);

    const checkJoinRequest = async () => {
        try {
            const token = sessionStorage.getItem("token");
            const res = await axios.get(`http://localhost:5082/api/teams/${id}/has-request`, {
                headers: { Authorization: `Bearer ${token}` }
            });
            setHasRequested(res.data);
        } catch (err) {
            console.error("Ошибка при проверке заявки:", err);
        }
    };

    const handleJoinRequest = async () => {
        try {
            const token = sessionStorage.getItem("token");
            await axios.post(`http://localhost:5082/api/teams/${id}/join`, { }, {
                headers: { Authorization: `Bearer ${token}` }
            });
            alert('Заявка отправлена!');
            setHasRequested(true);
        } catch (error) {
            console.error("Ошибка при отправке заявки:", error);
            alert('Ошибка при подаче заявки');
        }
    };

    const handleBanTeam = async () => {
        if (!banReason.trim()) {
            alert("Введите причину бана");
            return;
        }
        try {
            const token = sessionStorage.getItem("token");
            await axios.post(`http://localhost:5082/api/teams/${id}/ban`,
                { reasonBan: banReason },
                { headers: { Authorization: `Bearer ${token}` } }
            );
            alert("Команда заблокирована");
        } catch (error) {
            console.error("Ошибка при блокировке команды:", error);
            alert("Ошибка при блокировке команды");
        }
    };

    if (!team) return <div>Загрузка...</div>;

    if (team?.ban === true && !isAdmin && team?.creatorId !== currentUserId) {
        return (
            <div style={{ padding: "2rem", color: "red" }}>
                <h2>Эта команда была заблокирована администрацией</h2>
                <p>Причина: {team.reasonBan || 'не указана'}</p>
            </div>
        );
    }

    return (
        <div>
            <h1>{team.name}</h1>
            <p><strong>Город:</strong> {team.city}</p>
            <p><strong>Возраст:</strong> {team.age}</p>
            <p><strong>Вид спорта:</strong> {team.sport}</p>
            <p><strong>Создатель:</strong> <Link to={`/profile/${team.creatorId}`}>{team.creator}</Link></p>
            {team.creatorId === currentUserId && (
                <Link to={`/team/${team.id}/edit`}>
                    <button>Редактировать команду</button>
                </Link>
            )}
            {isAdmin && (
                <div style={{ marginTop: "20px", borderTop: "1px solid gray", paddingTop: "10px" }}>
                    <h3>Блокировка команды</h3>
                    <textarea
                        placeholder="Причина блокировки"
                        value={banReason}
                        onChange={(e) => setBanReason(e.target.value)}
                        rows={3}
                        style={{ width: "100%", marginBottom: "10px" }}
                    />
                    <button onClick={handleBanTeam}>Заблокировать команду</button>
                </div>
            )}
            {!hasRequested ? (
                <button onClick={handleJoinRequest}>Подать заявку в команду</button>):
                <p>Вы уже подали заявку в эту команду.</p>}

            <h3>Участники команды:</h3>
            {players.length === 0 ? (
                <p>Участников пока нет.</p>
            ) : (
                    <ul>
                        {players.map((player) => (
                            <li key={player.id}>
                                <Link to={`/profile/${player.id}`}>{player.userName}</Link>
                            </li>
                        ))}
                    </ul>
            )}

            <Link to={`/team/${id}/Entries`}>Просмотр заявок в команду</Link>
        </div>
    );
};

export default TeamDetails;
